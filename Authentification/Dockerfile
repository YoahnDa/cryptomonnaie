FROM php:8.3-cli

# Installe les dépendances PHP
RUN apt-get update && apt-get install -y \
    git unzip libpq-dev libzip-dev libonig-dev libxml2-dev \
    && docker-php-ext-install zip pdo_pgsql mbstring \
    && curl -sS https://getcomposer.org/installer | php -- --install-dir=/usr/local/bin --filename=composer

# Définir le répertoire de travail
WORKDIR /var/www/html

# Copier tous les fichiers
COPY . .

# Vérifier que bin/console est présent
RUN ls -l bin/console

# Nettoyer le cache Symfony
RUN rm -rf var/cache/*

# Installer les dépendances Symfony
RUN composer install --no-interaction --optimize-autoloader --no-progress \
    && composer dump-autoload --no-dev --optimize

RUN php bin/console doctrine:database:create --if-not-exists

# Exécuter les migrations automatiquement
RUN php bin/console doctrine:migrations:migrate --no-interaction

# Définir les permissions
RUN chown -R www-data:www-data /var/www/html

# Exposer le port 8000 (port par défaut du serveur Symfony intégré)
EXPOSE 8000

# Exécuter Symfony avec son serveur intégré
CMD php -S 0.0.0.0:8000 -t public
