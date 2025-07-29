import CryptoJS from 'crypto-js';

const decryptlogID = (encryptedItem) => {
    try {
        const bytes = CryptoJS.AES.decrypt(encryptedItem, import.meta.env.VITE_SECRET_KEY || 'thisismytestsecretkey');
        const decryptedString = bytes.toString(CryptoJS.enc.Utf8);
        return decryptedString;
    } catch (e) {
        return null;
    }
}

const getLoggedUserId = () => {
    const encryptedLogId = sessionStorage.getItem('logId');
    if (!encryptedLogId) return null;
    return decryptlogID(encryptedLogId);
}

export default getLoggedUserId;
