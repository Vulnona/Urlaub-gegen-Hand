FROM node:20-alpine as base

RUN apk add tzdata
ENV TZ=Europe/Berlin

WORKDIR /app/frontend

FROM base AS dev

RUN npm install -g npm
WORKDIR /app/frontend
COPY . .

RUN npm install

CMD ["npm", "run", "dev"]