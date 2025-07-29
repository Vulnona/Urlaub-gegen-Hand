import AES from 'crypto-js/aes';
import CryptoJS from 'crypto-js';

const SECRET_KEY = import.meta.env.VITE_SECRET_KEY || 'thisismytestsecretkey';

export const encryptItem = (item: string): string => {
  console.log('Encrypting item with centralized function');
  const encrypted = AES.encrypt(item, SECRET_KEY).toString();
  console.log('Encryption successful');
  return encrypted;
};

export const decryptItem = (encryptedItem: string): string | null => {
  try {
    console.log('Decrypting item with centralized function');

    const bytes = AES.decrypt(encryptedItem, SECRET_KEY);
    const decrypted = bytes.toString(CryptoJS.enc.Utf8);

    if (!decrypted) {
      console.warn('Decryption failed - empty result');
      return null;
    }

    console.log('Decryption successful');
    return decrypted;
  } catch (e) {
    console.warn('Decryption error:', e);
    return null;
  }
}; 