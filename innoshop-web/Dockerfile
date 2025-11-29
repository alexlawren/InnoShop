FROM node:20-alpine as build
WORKDIR /app

COPY package*.json ./

RUN npm config set fetch-retry-maxtimeout 600000 \
    && npm config set fetch-retry-mintimeout 10000 \
    && npm config set fetch-retries 5 \
    && npm config set fetch-timeout 600000

RUN npm install --legacy-peer-deps --verbose

COPY . .
RUN npm run build

FROM nginx:alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/innoshop-web/browser /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]