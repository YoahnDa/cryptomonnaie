# Utiliser l'image officielle de Node.js comme image de base
FROM node:14

# Définir le répertoire de travail dans le conteneur
WORKDIR /app

# Copier le fichier package.json et package-lock.json dans le répertoire de travail
COPY package*.json ./

# Installer les dépendances du projet
RUN npm install

# Copier tout le contenu du projet dans le répertoire de travail
COPY . .

# Construire l'application Vue.js pour la production
RUN npm run build

# Utiliser une image de serveur web pour servir l'application
FROM nginx:alpine

# Copier les fichiers de build dans le répertoire de Nginx
COPY --from=0 /app/dist /usr/share/nginx/html

# Exposer le port 80
EXPOSE 80

# Démarrer Nginx
CMD ["nginx", "-g", "daemon off;"]