﻿using Domain.Commands;
using Domain.ViewModel;

namespace Domain.Interface
{
    public interface IEntregadorService
    {
        Task<string> PostEntregadorAsync(EntregadorCommand command);
        Task<int> ValidaCNPJ(string cnpj);
        Task<string> ValidaCNH(string cnh);
        Task<string> PutImageAsync(string cnpj, ImagemCommand imagemCommand);
        Task<string> SalvarFotoCNHAsync(string nomeArquivo, Stream fotoCNH);
        Task<Stream> RecuperarFotoCNHAsync(string nomeArquivo);
        Task<string> PostAlugarAsync(int plano, string cnpj, DateTime dataInicio, DateTime dataDevolucao);
        Task<AlugarVeiculoViewModel> SimularAluguel(int plano, string cnpj, DateTime dataInicio, DateTime dataDevolucao);
        Task<bool> VerificaDataMaior(DateTime dataInicio, DateTime dataDevolucao);        
        Task<decimal> GetValorDiaria(int plano);
        Task<bool> GetVeiculosDisponiveis();
    }
}
