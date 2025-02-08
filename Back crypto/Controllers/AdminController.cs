using Backend_Crypto.Dto;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;
using Backend_Crypto.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend_Crypto.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPorteFeuilleRepository _porteFeuilleRepository;
        private readonly UserAnalytique _analytique;
        public AdminController (ITransactionRepository transactionRepository, UserAnalytique userAnalytique, IPorteFeuilleRepository portefeuille)
        {
            _transactionRepository = transactionRepository;
            _porteFeuilleRepository = portefeuille;
            _analytique = userAnalytique;
        }

        [HttpGet("transaction/exchangefond")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDtoAdmin>))]
        [ProducesResponseType(500)]
        public IActionResult GetExchangeInfo()
        {
            var listType = new List<TypeTransaction> { TypeTransaction.Retrait, TypeTransaction.Depot };
            return Ok(_analytique.getTransac(listType));
        }

        [HttpGet("transaction/exchangefond/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDtoAdmin>))]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        public IActionResult GetExchangeUser(int id)
        {
            var listType = new List<TypeTransaction> { TypeTransaction.Retrait, TypeTransaction.Depot };
            if (_porteFeuilleRepository.PortefeuilleExiste(id))
            {
                return NotFound("Portefeuille introuvable.");
            }
            var portefeuille = _porteFeuilleRepository.GetPortefeuille(id);
            return Ok(_analytique.getTransacUser(listType,portefeuille.IdPortefeuille));
        }

        [HttpGet("transaction/operation")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDtoAdmin>))]
        [ProducesResponseType(500)]
        public IActionResult GetOperationInfo()
        {
            var listType = new List<TypeTransaction> { TypeTransaction.Vente, TypeTransaction.Achat };
            return Ok(_analytique.getTransac(listType));
        }

        [HttpGet("transaction/operation/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDtoAdmin>))]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        public IActionResult GetOperationUser(int id)
        {
            var listType = new List<TypeTransaction> { TypeTransaction.Vente, TypeTransaction.Achat };
            if (_porteFeuilleRepository.PortefeuilleExiste(id))
            {
                return NotFound("Portefeuille introuvable.");
            }
            var portefeuille = _porteFeuilleRepository.GetPortefeuille(id);
            return Ok(_analytique.getTransacUser(listType, portefeuille.IdPortefeuille));
        }

        [HttpGet("transaction/validation/exchangefond/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public IActionResult ValidateTransac(int id)
        {
            var transaction = _transactionRepository.GetTransaction(id);
            if (transaction == null)
            {
                return NotFound("Transaction introuvable.");
            }else if (!_porteFeuilleRepository.PortefeuilleExiste(transaction.PortefeuilleOwner.IdUser))
            {
                return NotFound("Portefeuille introuvable.");
            }

            var portefeuille = transaction.PortefeuilleOwner;

            if(transaction.Type == TypeTransaction.Depot)
            {
                portefeuille.Fond += transaction.fond;
                _porteFeuilleRepository.UpdatePortefeuille(portefeuille);
            }else if(transaction.Type == TypeTransaction.Retrait)
            {
                if (!_porteFeuilleRepository.HaveEnoughFond(portefeuille.IdUser, transaction.fond))
                {
                    return Unauthorized("Pas assez de fond.");
                }
                portefeuille.Fond -= transaction.fond;
            }
            _porteFeuilleRepository.UpdatePortefeuille(portefeuille);
            _transactionRepository.ChangeEtat(transaction, Status.Valid);
            return Ok("Transaction valider.");
        }

        [HttpGet("transaction/annulation/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        public IActionResult AnnulationTransac(int id)
        {
            var transaction = _transactionRepository.GetTransaction(id);
            if (transaction == null)
            {
                return NotFound("Transaction introuvable.");
            }
            else if (!_porteFeuilleRepository.PortefeuilleExiste(transaction.PortefeuilleOwner.IdUser))
            {
                return NotFound("Portefeuille introuvable.");
            }
            _transactionRepository.ChangeEtat(transaction,Status.Annuler);
            return Ok("Transaction annuler.");
        }

        [HttpGet("transaction/recherche/exchangefond")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDtoAdmin>))]
        [ProducesResponseType(500)]
        public IActionResult Search([FromQuery] List<int>? idUtilisateurs,[FromQuery] List<int>? idCryptos)
        {
            var filter = new TransactionFilterDto
            {
                IdUtilisateurs = idUtilisateurs ?? new List<int>(),
                IdCryptos = idCryptos ?? new List<int>(),
                Types = new List<TypeTransaction>() { TypeTransaction.Retrait, TypeTransaction.Depot },
            };
            return Ok(_analytique.GetTransactionsByFilter(filter));
        }

        [HttpGet("transaction/recherche/operation")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDtoAdmin>))]
        [ProducesResponseType(500)]
        public IActionResult SearchOperation([FromQuery] List<int>? idUtilisateurs, [FromQuery] List<int>? idCryptos)
        {
            var filter = new TransactionFilterDto
            {
                IdUtilisateurs = idUtilisateurs ?? new List<int>(),
                IdCryptos = idCryptos ?? new List<int>(),
                Types = new List<TypeTransaction>() { TypeTransaction.Vente, TypeTransaction.Achat },
            };
            return Ok(_analytique.GetTransactionsByFilter(filter));
        }
    }
}
