<template>
    <Navbar />
    <div class="inner_banner_layout">
        <div class="container">
            <div class="row">
                <div class="col-sm-12">
                    <div class="inner_banner">
                        <h2>Membership Plans</h2>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <section class="plans-container">
        <div v-for="plan in plans" :key="plan.membershipID" class="plan-card">
            <img src="images/vip-membership.jpg" class="" />
            <h2 class="plan-name">{{ plan.name }}</h2>
            <p class="plan-description">{{ plan.description }}</p>
            <p class="plan-price">Price: ${{ plan.price.toFixed(2) }} / month</p>
            <p class="plan-duration">Duration: {{ plan.durationMonths }} month(s)</p>
           <div>
            <button class="subscribe-button">Subscribe</button>
           </div> 
        </div>
    </section>


</template>

<script setup>
import Navbar from '@/components/navbar/Navbar.vue';
import axiosInstance from '@/interceptor/interceptor';
import { onMounted, ref } from 'vue';

const plans = ref([]);

onMounted(fetchPlans);

async function fetchPlans() {
    try {
        const response = await axiosInstance.get(`membership/get-all-memberships`);
        plans.value = response.data;
    } catch (error) {
        console.error(error);
    }
}
</script>

<style scoped>
img {max-width: 100%;}
.plans-container {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    gap: 20px;
    padding: 20px;
    background-color: #f7f9fc;
}

.plan-card {
    background-color: #fff;
    border-radius: 10px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    width: 300px;
    padding: 20px;
    text-align: center;
    transition: transform 0.2s;
}

.plan-card:hover {
    transform: translateY(-10px);
}

.plan-name {
    font-size: 1.5rem;
    color: #333;
    margin-bottom: 10px;
}

.plan-description {
    font-size: 1rem;
    color: #555;
    margin-bottom: 15px;
}

.plan-price,
.plan-duration {
    font-size: 1.1rem;
    color: #444;
    margin-bottom: 10px;
}

.subscribe-button {
    background-color: #007bff;
    color: white;
    border: none;
    padding: 10px 20px;
    border-radius: 5px;
    cursor: pointer;
    font-size: 1rem;
    transition: background-color 0.2s;
}

.subscribe-button:hover {
    background-color: #0056b3;
}
</style>