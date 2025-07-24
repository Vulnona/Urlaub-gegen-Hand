<template>
    <div class="main_header" v-if="isLoggedIn">
        <div class="container">
            <div class="row">
                <div class="col-sm-12">
                    <nav class="navbar navbar-expand-lg navbar-white bg-white">
                        <a class="navbar-brand" href="/home">
                            <img src="@/assets/images/logo/logo-desktop.svg" class="logo" alt="Logo">
                        </a>
                        <button class="navbar-toggler" type="button" data-toggle="collapse"
                            data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent"
                            aria-expanded="false" aria-label="Toggle navigation">
                            <span class="navbar-toggler-icon"></span>
                        </button>
                        <div class="collapse navbar-collapse" id="navbarSupportedContent">
                            <ul class="navbar-nav ml-auto">                                
                                <li class="nav-item">
                                    <router-link class="nav-link" to="/login">
                                        <i class="ri-login-box-line"></i> Login
                                    </router-link>
                                </li>
                                <li class="nav-item">
                                    <router-link class="nav-link" to="/register">
                                        <i class="ri-login-box-line"></i> Registrieren
                                    </router-link>
                                </li>
                            </ul>
                        </div>
                    </nav>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue';

const isLoggedIn = ref(false);


onMounted(async () => {
    isLoggedIn.value = !sessionStorage.getItem("token");
});
</script>

<style lang="scss" scoped>
.logo {
    inline-size: 200px;
    margin-bottom: 10px;

    @media (min-width: 769px) {
        margin-bottom: 0;
    }
}

.nav-list {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    align-items: center;
    list-style: none;
    padding: 0;
    margin: 0;

    @media (max-width: 768px) {
        flex-direction: column;
    }
}
</style>
