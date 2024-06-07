FROM node:20-alpine as base

# Set the timezone
RUN apk add --no-cache tzdata \
    && cp /usr/share/zoneinfo/Europe/Berlin /etc/localtime \
    && echo "Europe/Berlin" > /etc/timezone \
    && apk del tzdata

# Set the working directory
WORKDIR /app/frontend

# Use a separate stage for development dependencies and setup
FROM base AS dev

# Install npm globally
RUN npm install -g npm

# Set the working directory again in the new stage
WORKDIR /app/frontend

# Copy the application files
COPY . .

# Install dependencies
RUN npm install

# Ensure vite binary is executable
RUN chmod +x ./node_modules/.bin/vite

# Run the development server
CMD ["npm", "run", "dev"]
