import router from '@/router';

const Securitybot=()=> {
    if (!sessionStorage.getItem("token")) {
      router.push('/login');
    }
};

export default Securitybot;