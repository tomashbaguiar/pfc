FROM node AS build
WORKDIR /usr/local/app
COPY . .
RUN npm install --force
RUN npm run build --prod

FROM nginx
COPY --from=build /usr/local/app/dist/monitoring-app /usr/share/nginx/html
COPY /nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
