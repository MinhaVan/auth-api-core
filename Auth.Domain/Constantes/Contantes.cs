namespace Auth.Domain.Constantes;

public static class Contantes
{
    public static class Cache
    {
        public const string ChaveLogin = "login:{cpf}:{email}:{senha}:{empresaId}:{isMotorista}";
        public const string ChaveBuscarPorCpfEmpresa = "buscarPorCpfEmpresa:cpf:{cpf}:{empresaId}";
    }
}