#!/bin/bash

# Attendre PostgreSQL (remplace `db` par le nom de ton service PostgreSQL)
echo "Attente de PostgreSQL..."
until nc -z -v -w30 db 5432; do
  echo "En attente de la base de données..."
  sleep 1
done

# Exécuter les commandes Symfony
php bin/console doctrine:database:create --if-not-exists
php bin/console doctrine:migrations:migrate --no-interaction

# Lancer Apache
exec apache2-foreground
