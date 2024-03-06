FROM node:20-alpine as base

RUN apk add tzdata
ENV TZ=Europe/Berlin

WORKDIR /app/frontend

COPY . .

RUN npm install -g npm

FROM base AS dev
WORKDIR /app/frontend

RUN npm install

CMD ["npm", "run", "dev"]