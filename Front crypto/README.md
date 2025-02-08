Voici le détail des fonctionnalités demandées :

---

## **🔑 Authentification et Gestion de Compte**
### **1. Login**
- L’utilisateur se connecte avec son **email et mot de passe**.
- Si échec, le nombre de tentatives est réduit.
- Après **3 tentatives incorrectes** (paramétrable), le compte est temporairement bloqué.
- En cas de succès, le nombre de tentatives est remis à **zéro**.

### **2. Vérification PIN (MFA)**
- Après connexion, l’utilisateur reçoit un **PIN temporaire** (90 secondes) par email.
- Il doit entrer ce PIN pour finaliser l’authentification.
- Si le PIN expire, un nouveau peut être demandé.

### **3. Inscription**
- Création d’un compte avec **email et mot de passe**.
- **Validation de l’email** obligatoire avant l’accès à la plateforme.

---

## **📊 Tableau de Bord et Gestion du Portefeuille**
### **4. Tableau de bord portefeuille**
- Vue d’ensemble de l’état du portefeuille crypto de l’utilisateur :
  - **Solde total** en fiat.
  - **Liste des cryptos possédées** et leur valeur actuelle.
  - **Historique des dernières transactions**.

---

## **💰 Transactions (Achat, Vente, Dépôt, Retrait)**
### **5. Achat**
- Sélection de la cryptomonnaie à acheter.
- Saisie du montant en fiat.
- Validation de l’achat (fond suffisant requis).
- Conversion en cryptomonnaie et ajout au portefeuille.

### **6. Vente**
- Sélection de la cryptomonnaie à vendre.
- Saisie du montant en crypto.
- Conversion en fond fiat et mise à jour du portefeuille.

### **7. Dépôt**
- Demande d’ajout de fonds au portefeuille.
- Validation via email.
- En attente de confirmation par l’**admin** (via back-office).

### **8. Retrait**
- Demande de retrait de fonds.
- Validation via email.
- En attente de confirmation par l’**admin**.

### **9. Validation (Admin) ⇒ Dépôt et Retrait**
- L’admin valide les **demandes de dépôt et de retrait**.
- Une **notification mobile** est envoyée à l’utilisateur après validation.

---

## **📈 Suivi des Transactions et Cours**
### **10. Cours en temps réel**
- Affichage des **cours des cryptos** avec mise à jour toutes les **10 secondes**.
- Génération aléatoire des cours pour simuler le marché.

### **11. Page historique de vente et achat**
- Liste des transactions passées.
- **Filtres** : date, utilisateur, cryptomonnaie.
- Affichage de **l’image de profil** des utilisateurs.

### **12. Page d’analyse crypto**
- Sélection d’un **type d’analyse** :
  - 1er quartile
  - Max / Min
  - Moyenne
  - Écart-type
- Filtrage par **date, heure et cryptomonnaie**.

### **13. Page d’analyse de commissions**
- Sélection d’un **type d’analyse** :
  - Somme des commissions
  - Moyenne des commissions
- Filtrage par **cryptomonnaie et date**.

### **14. Page pour voir la somme totale d’achat et de vente effectués**
- Tableau récapitulatif avec total des achats et ventes par utilisateur.
- **Filtre par date**.

---

## **🔧 Gestion Admin et Historique**
### **15. Page validation dépôt et retrait utilisateurs (Admin)**
- Liste des demandes en attente.
- Validation ou rejet des demandes.
- **Notification mobile** envoyée après validation.

### **16. Page historique des opérations pour l’utilisateur**
- Affichage des **transactions personnelles**.
- Filtres : **date, cryptomonnaie**.







ameliorer et centrer le css seulement de cet template avec animation possible:










