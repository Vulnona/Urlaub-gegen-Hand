const express = require('express');
const path = require('path');
const app = express();
const PORT = 3002;
const fs = require('node:fs');
const axios = require('axios');

//environment variable or default to localhost for API URL
const API_BASE_URL = 'https://alreco.de:8443';

const url = `${API_BASE_URL}/api/offer/get-offer-by-id/`;
const headers = {'Content-Type': 'application/json'};

let distPath = path.join('/app/dist');
let index;
fs.readFile(distPath + '/index.html', 'utf8', (err, data) => {
  if (err) {
    console.error(err);
    return;
  }
    index = data;
});

app.use(express.static(distPath));
app.get('/:loc', function (req,res) {
    res.sendFile(distPath + "/index.html");
});
app.get('/offer/:id', (req, res, next) => {
    let id = req.params.id;
    //dynamic URL instead of the hardcoded one
    let i2 = index.replace(/<meta property="og:image" (.*?)>/, `<meta property="og:image" content="${API_BASE_URL}/api/offer/get-preview-picture/${req.params.id}">`);
    axios.get(`${url}${id}`, { headers })
        .then(ret => {
            i2 = i2.replace(/<meta property="og:title" (.*?)>/, `<meta property="og:title" content="${ret.data.title}">`);
            i2 = i2.replace(/<meta property="og:description" (.*?)>/, `<meta property="og:description" content="${ret.data.description}">`);
            res.send(i2);
            })        
    .catch(error => {
        console.error('Error:', error.message);
        res.send(i2);
    });
});

app.listen(PORT, () => {
    console.log(`Server Established at PORT -> ${PORT}`);
});

