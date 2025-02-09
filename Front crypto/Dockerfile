# Étape 1 : Build Vue.js avec une image légère
FROM node:20-alpine AS build

# Définir le répertoire de travail
WORKDIR /app

# Copier uniquement les fichiers nécessaires pour l’installation
COPY package.json package-lock.json ./

# Installer les dépendances uniquement pour la production
RUN npm ci --only=production

# Copier le reste des fichiers (code source)
COPY . .

# Compiler Vue.js (en une seule étape pour réduire le nombre de couches)
RUN npm ci && npm run build

# Étape 2 : Servir l’application avec Nginx
FROM nginx:alpine

# Copier uniquement les fichiers de build dans l’image finale
COPY --from=build /app/dist /usr/share/nginx/html

# Exposer le port 80
EXPOSE 80

# Démarrer Nginx
CMD ["nginx", "-g", "daemon off;"]
