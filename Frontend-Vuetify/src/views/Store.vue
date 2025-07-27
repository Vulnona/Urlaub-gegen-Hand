<template>
  <Navbar />
  <div class="inner_banner_layout">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div class="inner_banner">
            <h2>Coupon Store</h2>
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- Stripe Payment Modal -->
  <div class="modal" :class="{ 'show d-block': showStripeModal }" tabindex="-1">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Complete Payment</h5>
          <button type="button" class="btn-close" @click="closeStripeModal"></button>
        </div>
        <div class="modal-body">
          <div id="payment-element"></div>
          <div id="payment-message" class="hidden"></div>
          <button id="submit" class="btn btn-primary w-100 mt-4" :disabled="isPaymentProcessing">
            <span v-if="isPaymentProcessing">
              Processing...
            </span>
            <span v-else>
              Pay now
            </span>
          </button>
        </div>
      </div>
    </div>
  </div>

  <section class="section_space offers_list">
    <div class="container">
      <div class="row">
        <div class="col-sm-12">
          <div v-if="ShopItems.length" class="offers_group">
            <div v-if="loading" class="spinner-container text-center">
              <div class="spinner"></div>
            </div>
            <div v-else class="row">
              <div v-for="item in ShopItems" :key="item.id" class="col-md-3 mb-4">
                <div class="card shadow-sm">
                  <div class="item_img">
                    <img
                      :src="item.image || 'https://imgs.search.brave.com/BJWlW1TAB1RZa2wlXEHFYLKXlgXW5I6VoCmsWOWF9yU/rs:fit:860:0:0:0/g:ce/aHR0cHM6Ly9jZG4u/dmVjdG9yc3RvY2su/Y29tL2kvcHJldmll/dy0xeC82Ny85MC9z/aG9wcGluZy1wcmlj/ZS10YWctZGlzY291/bnQtY291cG9uLXdp/dGgtcGVyY2VudC12/ZWN0b3ItNDE5NDY3/OTAuanBn'"
                      alt="Offer Image" class="card-img-top" />
                  </div>
                  <div class="card-body" style="z-index: 9;">
                    <h3 class="card-title">{{ item.name }}</h3>
                    <p class="card-text">
                      <strong>Beschreibung:</strong> {{ item.description }}
                    </p>
                    <p class="card-text">
                      <strong>Preis:</strong> {{ item.price.amount }}
                      {{ item.price.currency }}
                    </p>
                    <button class="btn btn-success btn-block" @click="confirmBuy(item)">
                      Kaufen
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div v-else>
            <h2 class="text-center">No Coupons Found!</h2>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import { loadStripe } from '@stripe/stripe-js';
import Swal from "sweetalert2";
import Navbar from "@/components/navbar/Navbar.vue";
import axiosInstance from "@/interceptor/interceptor";
import Securitybot from "@/services/SecurityBot";

let stripe = null;
let elements = null;

export default {
  components: {
    Navbar,
  },
  data() {
    return {
      loading: true,
      ShopItems: [],
      showStripeModal: false,
      isPaymentProcessing: false,
      currentItemId: null
    };
  },
  async mounted() {
    Securitybot();
    this.fetchShopItems();
    stripe = await loadStripe(`${process.env.StripeKey}`);
  },

  methods: {
    async fetchShopItems() {
      try {
        const response = await axiosInstance.get(`ShopItem`);
        this.ShopItems = response.data.items;
        this.loading = false;
      } catch (error) {
        console.error('Fehler beim Laden der Shop-Items:', error);
        Swal.fire("Fehler", "Shop-Items konnten nicht geladen werden. Bitte versuchen Sie es erneut.", "error");
      }
    },
    async confirmBuy(item) {
      const result = await Swal.fire({
        title: `Kauf bestätigen`,
        text: `Sind Sie sicher, dass Sie ${item.name} für ${item.price.amount} ${item.price.currency} kaufen möchten?`,
        showCancelButton: true,
        confirmButtonText: "Ja, kaufen!",
        cancelButtonText: "Abbrechen",
      });

      if (result.isConfirmed) {
        this.currentItemId = item.id;
        this.buy(item.id);
      }
    },
    async buy(ShopitemId) {
      try {
        const response = await axiosInstance.post(`create-payment-intent`, {
          ShopitemId,
          automatic_payment_methods: { enabled: true },
          payment_method_types: ['card', 'paypal']
        });

        if (response.status === 200 && response.data.clientSecret) {
          console.log('Payment intent created with client secret');
          await this.initializeStripeElements(response.data.clientSecret);
          this.showStripeModal = true;
        } else {
          Swal.fire("Fehler", "Etwas ist schief gelaufen. Bitte versuchen Sie es erneut.", "error");
        }
      } catch (error) {
        console.error('Fehler beim Erstellen des Payment-Intents:', error);
        Swal.fire("Fehler", "Kauf konnte nicht verarbeitet werden. Bitte versuchen Sie es erneut.", "error");
      }
    },
    async initializeStripeElements(clientSecret) {
      try {
        // Clean up existing elements
        if (elements) {
          const existingElement = elements.getElement('payment');
          if (existingElement) {
            existingElement.destroy();
          }
        }

        console.log('Setting up Stripe elements with PayPal support');
        elements = stripe.elements({
          clientSecret,
          appearance: {
            theme: 'stripe',
            variables: {
              colorPrimary: '#0066cc',
              borderRadius: '4px'
            }
          }
        });

        const paymentElement = elements.create('payment', {
          layout: {
            type: 'tabs',
            defaultCollapsed: false
          },
          fields: {
            billingDetails: 'auto'
          },
          paymentMethodOrder: ['card', 'paypal']
        });

        paymentElement.mount('#payment-element');

        const form = document.querySelector('#submit');
        form.addEventListener('click', this.handleSubmit);
      } catch (error) {
        console.error('Fehler beim Initialisieren der Stripe-Elemente:', error);
        Swal.fire("Fehler", "Zahlungsformular konnte nicht geladen werden. Bitte versuchen Sie es erneut.", "error");
      }
    },
    async handleSubmit(e) {
      e.preventDefault();
      this.isPaymentProcessing = true;

      const { error } = await stripe.confirmPayment({
        elements,
        confirmParams: {
          return_url: `${window.location.origin}/purchase-history?success=true`,
          payment_method_data: {
            billing_details: {
              name: '',
              email: ''
            }
          }
        }
      });

      if (error) {
        console.error('Payment confirmation error:', error);
        const messageDiv = document.querySelector('#payment-message');
        messageDiv.textContent = 'Zahlung fehlgeschlagen. Bitte versuchen Sie es erneut.';
        messageDiv.classList.remove('hidden');
        this.isPaymentProcessing = false;
      }
      // Payment successful - Stripe will redirect to return_url
    },
    closeStripeModal() {
      this.showStripeModal = false;
      this.isPaymentProcessing = false;
      this.currentItemId = null;
      if (elements) {
        const paymentElement = elements.getElement('payment');
        if (paymentElement) {
          paymentElement.destroy();
        }
      }
    },
  },
};
</script>

<style>
.inner_banner_layout {
  background: url("/images/offerRequest_bg.webp") no-repeat center center;
  background-size: cover;
  padding: 60px 0;
  text-align: center;
  color: #fff;
}

.offers_list .card {
  border: none;
  border-radius: 8px;
  transition: transform 0.3s;
}

.offers_list .card:hover {
  transform: scale(1.05);
}

.offers_list .card-img-top {
  border-top-left-radius: 8px;
  border-top-right-radius: 8px;
}

.offers_list .btn {
  margin-top: 15px;
}

.modal {
  background-color: rgba(0, 0, 0, 0.5);
}

#payment-message {
  color: rgb(105, 115, 134);
  font-size: 16px;
  line-height: 20px;
  padding-top: 12px;
  text-align: center;
}

#payment-message.hidden {
  display: none;
}

#payment-element {
  margin-bottom: 24px;
}
</style>
