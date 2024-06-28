<template>
    <div class="resend-email-verification">
        <h2>Email Verification</h2>
        <form @submit.prevent="VerificationEmail" class="form-container">
            <div class="form-group">
                <label for="email">Email:</label>
                <input type="email" v-model="email" id="email" required />
            </div>
            <button type="submit" class="submit-button">Send Verification Email</button>
            <a href="/" class="text-center">Back</a>
        </form>
        <div v-if="message" :class="{ 'error-message': isError, 'success-message': !isError }">
            {{ message }}
        </div>
    </div>
</template>

<script>
import axios from 'axios';

export default {
    data() {
        return {
            email: '',
            message: '',
            isError: false
        };
    },
    methods: {
        async VerificationEmail() {
            try {
                const response = await axios.post(`${process.env.baseURL}auth/resend-email-verification`, {
                    email: this.email
                });
                this.message = response.data;
                this.isError = false;
            } catch (error) {
                if (error.response) {
                    this.message = error.response.data;
                    this.isError = true;
                } else {
                    this.message = 'An error occurred. Please try again.';
                    this.isError = true;
                }
            }
        }
    }
};
</script>

<style scoped>
.resend-email-verification {
    max-width: 600px;
    margin: auto;
    padding: 30px;
    background-color: #f9f9f9;
    border-radius: 8px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

h2 {
    text-align: center;
    color: #333;
    margin-bottom: 20px;
}

.form-container {
    display: flex;
    flex-direction: column;
    gap: 15px;
}

.form-group {
    display: flex;
    flex-direction: column;
}

label {
    font-weight: bold;
    margin-bottom: 5px;
    color: #555;
}

input[type="email"] {
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 4px;
    font-size: 16px;
}

input[type="email"]:focus {
    border-color: #007BFF;
    outline: none;
}

.submit-button {
    padding: 10px;
    background-color: #007BFF;
    color: #fff;
    border: none;
    border-radius: 4px;
    font-size: 16px;
    cursor: pointer;
    transition: background-color 0.3s ease;
}

.submit-button:hover {
    background-color: #0056b3;
}

.error-message {
    color: #ff4d4d;
    text-align: center;
    margin-top: 20px;
}

.success-message {
    color: #28a745;
    text-align: center;
    margin-top: 20px;
}
</style>