using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp_Desafio_API.ViewModels;
using WebApp_Desafio_BackEnd.Business;
using System.Globalization;

namespace WebApp_Desafio_API.Controllers
{
    /// <summary>
    /// ChamadosController
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ChamadosController : Controller
    {
        private ChamadosBLL bll = new ChamadosBLL();

        /// <summary>
        /// Lista todos os chamados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ChamadoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [Route("Listar")]
        public IActionResult Listar()
        {
            try
            {
                var _lst = this.bll.ListarChamados();

                var lst = from chamado in _lst
                          select new ChamadoResponse()
                          {
                              id = chamado.ID,
                              assunto = chamado.Assunto,
                              solicitante = chamado.Solicitante,
                              idDepartamento = chamado.IdDepartamento,
                              departamento = chamado.Departamento,
                              dataAbertura = chamado.DataAbertura
                          };

                return Ok(lst);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(422, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Obtém dados de um chamado específico
        /// </summary>
        /// <param name="idChamado">O ID do chamado a ser obtido</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ChamadoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [Route("Obter")]
        public IActionResult Obter([FromQuery] int idChamado)
        {
            try
            {
                var _chamado = this.bll.ObterChamado(idChamado);

                var chamado = new ChamadoResponse()
                              {
                                  id = _chamado.ID,
                                  assunto = _chamado.Assunto,
                                  solicitante = _chamado.Solicitante,
                                  idDepartamento = _chamado.IdDepartamento,
                                  departamento = _chamado.Departamento,
                                  dataAbertura = _chamado.DataAbertura
                              };

                return Ok(chamado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(422, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Grava os dados de um chamado
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [Route("Gravar")]
        public IActionResult Gravar([FromBody] ChamadoRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException("Request não informado.");
                if (request.assunto == null)
                    throw new ArgumentNullException("Assunto não informado.");
                if (request.solicitante == null)
                    throw new ArgumentNullException("Solicitante não informado.");
                if (request.idDepartamento == 0)
                    throw new ArgumentNullException("ID do Departamento não informado.");
                if (request.dataAbertura == DateTime.MinValue)
                    throw new ArgumentNullException("Data de abertura não informada.");
                if (request.assunto.Length > 128)
                    throw new ArgumentException("O comprimento do Assunto é inválido. Deve ter no máximo 128 dígitos.");
                if (request.solicitante.Length > 64)
                    throw new ArgumentException("O comprimento do Solicitante é inválido. Deve ter no máximo 64 dígitos.");

                DateTime dataAtual = DateTime.Now;
                DateTime dataSelecionada = request.dataAbertura;
                DateTime dataSeguinte = dataSelecionada.AddDays(1);

                if (dataSeguinte < dataAtual)
                    throw new ArgumentException("A data selecionada é retroativa. Por favor, escolha uma data atual ou futura.");


                var resultado = this.bll.GravarChamado(request.id,
                                                       request.assunto,
                                                       request.solicitante,
                                                       request.idDepartamento,
                                                       request.dataAbertura);

                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(422, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        /// <summary>
        /// Exclui um chamado específico
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [Route("Excluir")]
        public IActionResult Excluir([FromQuery] int idChamado)
        {
            try
            {
                var resultado = this.bll.ExcluirChamado(idChamado);

                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(422, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
