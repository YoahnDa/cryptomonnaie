<?php

namespace App\Controller\Api;

use App\Repository\TokenRepository;
use App\Repository\UserRepository;
use App\Service\VerificationToken;
use Doctrine\ORM\EntityManagerInterface;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\Security\Core\Security;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\PasswordHasher\Hasher\UserPasswordHasherInterface;
use Symfony\Component\Routing\Attribute\Route;
use Symfony\Component\Serializer\SerializerInterface;
use Symfony\Component\Validator\Validator\ValidatorInterface;

class AdminController extends AbstractController
{
    /**
     * Get information des utilisateurs.
     * 
     * @Route("/api/admin/userInfo", name="app_api_user", methods={"GET"})
     * @OA\Get(
     *     path="/api/admin/userInfo",
     *     summary="Get information des utilisateurs",
     *     @OA\Response(
     *         response=200
     *     ),
     *     @OA\Response(
     *         response=401,
     *         description="Vous n'êtes pas un admin.",
     *         @OA\JsonContent(type="object")
     *     )
     * )
     */
    #[Route('/api/admin/userinfo', name: 'app_api_user_info', methods: ['GET'])]
    public function getInfo(Request $request, UserRepository $userRepos, TokenRepository $tokenRepos, SerializerInterface $serializer, VerificationToken $verifSecurity): JsonResponse
    {
        $user = $verifSecurity->verifyToken($request, $tokenRepos);
        if (!$user) {
            return new JsonResponse(['message' => 'Vous n\'avez pas l\'autorisation'], Response::HTTP_UNAUTHORIZED);
        } elseif (!in_array('ROLE_ADMIN', $user->getRoles(), true)) {
            return new JsonResponse(['message' => 'Accès refusé, vous devez être administrateur'], Response::HTTP_FORBIDDEN);
        }
        $users = $userRepos->findAll(); // Récupère tous les utilisateurs
        $nonAdminUsers = array_filter($users, function ($user) {
            return !in_array('ROLE_ADMIN', $user->getRoles());
        });
        // Sérialiser les utilisateurs non-admins en JSON avec le groupe 'getUser'
        $userJson = $serializer->serialize(array_values($nonAdminUsers), 'json', ['groups' => 'getUser']);
        return new JsonResponse($userJson, Response::HTTP_OK, [], true);
    }

    /**
     * Get information des utilisateurs par ID.
     * 
     * @Route("/api/admin/userInfo/{id}", name="app_api_user_info", methods={"GET"})
     * @OA\Get(
     *     path="/api/admin/userInfo/{id}",
     *     summary="Get information d'un utilisateur par ID",
     *     @OA\Parameter(
     *         name="id",
     *         in="path",
     *         description="ID de l'utilisateur",
     *         required=true,
     *         @OA\Schema(type="integer")
     *     ),
     *     @OA\Response(
     *         response=200,
     *         description="Informations de l'utilisateur"
     *     ),
     *     @OA\Response(
     *         response=401,
     *         description="Vous n'êtes pas un admin.",
     *         @OA\JsonContent(type="object")
     *     ),
     *     @OA\Response(
     *         response=404,
     *         description="Utilisateur non trouvé",
     *         @OA\JsonContent(type="object")
     *     )
     * )
     */
    #[Route('/api/admin/userinfo/{id}', name: 'app_api_user_info_inside', methods: ['GET'])]
    public function getInfoUser(int $id, Request $request, UserRepository $userRepos, TokenRepository $tokenRepos, SerializerInterface $serializer, VerificationToken $verifSecurity): JsonResponse
    {
        // Vérifie le token de l'utilisateur pour l'authentification
        $user = $verifSecurity->verifyToken($request, $tokenRepos);
        if (!$user) {
            return new JsonResponse(['message' => 'Vous n\'avez pas l\'autorisation'], Response::HTTP_UNAUTHORIZED);
        } elseif (!in_array('ROLE_ADMIN', $user->getRoles(), true)) {
            return new JsonResponse(['message' => 'Accès refusé, vous devez être administrateur'], Response::HTTP_FORBIDDEN);
        }

        // Récupère l'utilisateur par son ID
        $userToFetch = $userRepos->find($id);

        // Si l'utilisateur n'existe pas, renvoie une erreur 404
        if (!$userToFetch) {
            return new JsonResponse(['message' => 'Utilisateur non trouvé'], Response::HTTP_NOT_FOUND);
        }

        // Sérialiser l'utilisateur en JSON avec le groupe 'getUser'
        $userJson = $serializer->serialize($userToFetch, 'json', ['groups' => 'getUser']);

        // Retourner la réponse JSON
        return new JsonResponse($userJson, Response::HTTP_OK, [], true);
    }
}
