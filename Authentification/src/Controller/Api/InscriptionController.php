<?php

namespace App\Controller\Api;

use App\Entity\Email as EntityEmail;
use Symfony\Component\Mime\Email; // pour le composant Mailer
use App\Entity\Token;
use App\Entity\User;
use App\Repository\TokenRepository;
use App\Service\EmailProvider;
use App\Service\VerificationToken;
use DateTimeImmutable;
use Doctrine\ORM\EntityManagerInterface;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Mailer\MailerInterface;
use Symfony\Component\PasswordHasher\Hasher\UserPasswordHasherInterface;
use Symfony\Component\Routing\Attribute\Route;
use Symfony\Component\Routing\Generator\UrlGeneratorInterface;
use Symfony\Component\Serializer\SerializerInterface;
use Symfony\Component\Validator\Validator\ValidatorInterface;
use Lexik\Bundle\JWTAuthenticationBundle\Services\JWTTokenManagerInterface;

class InscriptionController extends AbstractController
{

    /**
     * Insertion des information pour avoir l'email de confirmation.
     * 
     * @Route("/api/inscription", name="app_api_inscription", methods={"POST"})
     * @OA\Post(
     *     path="/api/inscription",
     *     summary="Inscription d'un utilisateur",
     *     @OA\RequestBody(
     *         required=true,
     *         @OA\JsonContent(
     *             type="object",
     *             @OA\Property(property="email", type="string", example="user@example.com"),
     *             @OA\Property(property="password", type="string", example="password123"),
     *             @OA\Property(property="username", type="string", example="username123")
     *         )
     *     ),
     *     @OA\Response(
     *         response=200,
     *         description="Email de vérification envoyé.",
     *         @OA\JsonContent(
     *             type="object",
     *             @OA\Property(property="message", type="string", example="Un email envoyer à user@example.com")
     *         )
     *     ),
     *     @OA\Response(
     *         response=400,
     *         description="Validation échouée.",
     *         @OA\JsonContent(type="object")
     *     )
     * )
     */
    #[Route('/api/inscription', name: 'app_api_inscription', methods: ['POST'])]
    public function inscription(Request $request, ValidatorInterface $validator, UserPasswordHasherInterface $passwordEncoder, SerializerInterface $serializer, VerificationToken $jwtManager, EntityManagerInterface $entity, UrlGeneratorInterface $urlGenerator, MailerInterface $mailer, EmailProvider $emailProvider): JsonResponse
    {
        $data = json_decode($request->getContent(), true);

        $email = new EntityEmail();
        $email->setValue($data['email'] ? $data['email'] : null);
        $email->setVerified(false);

        $errorEmail = $validator->validate($email);

        if ($errorEmail->count() > 0) {
            return new JsonResponse($serializer->serialize($errorEmail, 'json'), JsonResponse::HTTP_BAD_REQUEST, [], true);
        }

        $user = new User();
        $user->setIdEmail($email);
        $user->setRoles(["ROLE_USER"]);
        $user->setPassword($passwordEncoder->hashPassword($user, $data['password'] ? $data['password'] : ''));
        $user->setUsername($data['username'] ? $data['username'] : '');

        $errorUser = $validator->validate($user);

        if ($errorUser->count() > 0) {
            return new JsonResponse($serializer->serialize($errorUser, 'json'), JsonResponse::HTTP_BAD_REQUEST, [], true);
        }

        $entity->persist($email);
        $entity->persist($user);
        $entity->flush();

        $emailProvider->sendEmailVerification($mailer, $entity, $jwtManager, $user, $urlGenerator, $email);

        return new JsonResponse(['message' => 'Un email envoyer à ' . $email->getValue()], Response::HTTP_OK, []);
    }

    /**
     * Insertion des information admin.
     * 
     * @Route("/api/inscription", name="app_api_inscription", methods={"POST"})
     * @OA\Post(
     *     path="/api/inscription",
     *     summary="Inscription d'un utilisateur",
     *     @OA\RequestBody(
     *         required=true,
     *         @OA\JsonContent(
     *             type="object",
     *             @OA\Property(property="email", type="string", example="user@example.com"),
     *             @OA\Property(property="password", type="string", example="password123"),
     *             @OA\Property(property="username", type="string", example="username123")
     *         )
     *     ),
     *     @OA\Response(
     *         response=200,
     *         description="Email de vérification envoyé.",
     *         @OA\JsonContent(
     *             type="object",
     *             @OA\Property(property="message", type="string", example="Un email envoyer à user@example.com")
     *         )
     *     ),
     *     @OA\Response(
     *         response=400,
     *         description="Validation échouée.",
     *         @OA\JsonContent(type="object")
     *     )
     * )
     */
    #[Route('/api/inscription/admin', name: 'app_api_inscription_admin', methods: ['POST'])]
    public function inscriptionAdmin(Request $request, ValidatorInterface $validator, UserPasswordHasherInterface $passwordEncoder, SerializerInterface $serializer, VerificationToken $jwtManager, EntityManagerInterface $entity, UrlGeneratorInterface $urlGenerator, MailerInterface $mailer, EmailProvider $emailProvider): JsonResponse
    {
        $data = json_decode($request->getContent(), true);

        $email = new EntityEmail();
        $email->setValue($data['email'] ? $data['email'] : null);
        $email->setVerified(false);

        $errorEmail = $validator->validate($email);

        if ($errorEmail->count() > 0) {
            return new JsonResponse($serializer->serialize($errorEmail, 'json'), JsonResponse::HTTP_BAD_REQUEST, [], true);
        }

        $user = new User();
        $user->setIdEmail($email);
        $user->setRoles(["ROLE_ADMIN"]);
        $user->setPassword($passwordEncoder->hashPassword($user, $data['password'] ? $data['password'] : ''));
        $user->setUsername($data['username'] ? $data['username'] : '');

        $errorUser = $validator->validate($user);

        if ($errorUser->count() > 0) {
            return new JsonResponse($serializer->serialize($errorUser, 'json'), JsonResponse::HTTP_BAD_REQUEST, [], true);
        }

        $entity->persist($email);
        $entity->persist($user);
        $entity->flush();

        $emailProvider->sendEmailVerification($mailer, $entity, $jwtManager, $user, $urlGenerator, $email);

        return new JsonResponse(['message' => 'Admin insérer.Veuillez vérifier vos emails.'], Response::HTTP_OK, []);
    }

    /**
     *  Verification de l'email.
     * 
     * @Route("/api/verification/{token}", name="app_api_verification", methods={"GET"})
     * @OA\Get(
     *     path="/api/verification/{token}",
     *     summary="Vérification d'un email via un token",
     *     @OA\Parameter(
     *         name="token",
     *         in="path",
     *         description="Token de vérification",
     *         required=true,
     *         @OA\Schema(type="string")
     *     ),
     *     @OA\Response(
     *         response=202,
     *         description="Email validé avec succès.",
     *     ),
     *     @OA\Response(
     *         response=400,
     *         description="Erreur de validation ou expiration.",
     *         @OA\JsonContent(type="object")
     *     )
     * )
     */
    #[Route('/api/verification/{token}', name: 'app_api_verification', methods: ['GET'])]
    public function verification(Request $request, VerificationToken $jwtManager, EntityManagerInterface $entity): Response
    {
        $token = $request->get('token');

        if (!$token) {
            return $this->render('error/link_error.html.twig', [
                'message' => 'L\'url est endommagé.',
                'status_code' => JsonResponse::HTTP_BAD_REQUEST
            ]);
        }

        $tokenBase = $entity->getRepository(Token::class)->findValidToken($token, 'VER');

        if (!$tokenBase) {
            return $this->render('error/link_error.html.twig', [
                'message' => 'Token non valide.',
                'status_code' => JsonResponse::HTTP_BAD_REQUEST
            ]);
        }

        $userReal = $tokenBase->getIdUser();

        // Obtenir le timestamp actuel
        $dateCreate = new DateTimeImmutable();
        $currentTimestamp = time();
        $expirationTimestamp = ($tokenBase->getExpiredAt())->getTimestamp();

        // Vérifier si le token est expiré
        if ($currentTimestamp > $expirationTimestamp) {
            return $this->render('error/link_error.html.twig', [
                'message' => 'Session du lien est expiré.',
                'status_code' => JsonResponse::HTTP_BAD_REQUEST
            ]);
        }

        if (($userReal->getIdEmail())->isVerified()) {
            return $this->render('error/link_error.html.twig', [
                'message' => 'Votre email a déjà été vérifié.',
                'status_code' => JsonResponse::HTTP_BAD_REQUEST
            ]);
        }

        ($userReal->getIdEmail())->setVerified(true);

        if (!($entity->getRepository(Token::class)->findAuthToken($userReal))) {
            $payLoadAuth = [
                'id' => $userReal->getId(),
                'iat' => $dateCreate->getTimestamp()
            ];

            $tokenAuth = new Token();
            $tokenAuth->setIdUser($userReal);
            $tokenAuth->setType('AUTH');
            $tokenAuth->setCreatedAt($dateCreate);
            $tokenAuth->setToken($jwtManager->generateUniqueToken($userReal->getUserName()));

            $entity->persist($tokenAuth);
        }

        $tokenNew = $jwtManager->generateUniqueToken($userReal->getUserName());
        $tokenBase->setToken($tokenNew);
        $tokenBase->setUsed(true);

        $entity->persist($userReal);
        $entity->persist($tokenBase);
        $entity->flush();

        return $this->render('success/link_success.html.twig', [
            'message' => 'Votre lien de vérification a été utilisé avec succès.',
            'description' => 'Vous pouvez maintenant vous connectez !'
        ]);
    }
}
