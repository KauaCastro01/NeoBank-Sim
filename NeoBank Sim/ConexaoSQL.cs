using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace NeoBank_Sim
{
    class ConexaoSQL
    {
        //Constructor
        public SqlConnection cn = new SqlConnection("Data Source = LAPTOPZEMBER; INTEGRATED SECURITY = SSPI; INITIAL CATALOG = db_NeoBankSim");
        public SqlCommand cmd = new SqlCommand();
        public SqlDataReader dr;
        //Metodo para conectar ao banco de dados
        public void Conectar()
        {
            try { cn.Open(); }
            catch (Exception ex) { throw new Exception("\a\nErro ao conectar ao banco de dados: " + ex.Message); }
        }
        //Metodo para desconectar do banco de dados
        public void Desconectar()
        {
            try { cn.Close(); dr.Close(); }
            catch (Exception ex) { throw new Exception("\a\nErro ao conectar ao banco de dados: " + ex.Message); }
        }
        //Metodo para executar comandos SQL
        //Metodo para fazer para login
        public void EfetuarLogin(string cpf, string senha)
        {
            try
            {
                Conectar();
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT * FROM tbl_Usuario WHERE cpf = @cpf AND senha = @senha";
                cmd.Parameters.AddWithValue("cpf", cpf);
                cmd.Parameters.AddWithValue("senha", senha);
                cmd.Connection = cn;
                dr = cmd.ExecuteReader();
                if (dr.HasRows) { obterConta(cpf); }
                else { Console.WriteLine("\a\nUsuário não encontrado!"); Console.ReadKey(); }
            }
            catch (Exception Erro) { Console.WriteLine("\a\nErro: " + Erro.Message); Console.ReadKey(); }
            finally { Desconectar(); }
        }
        //Metodo para fazer o cadastro
        public void EfetuarCadastro(string nome, string cpf, string senha)
        {
            try
            {
                Conectar();
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT * FROM tbl_Usuario WHERE cpf = @cpf";
                cmd.Parameters.AddWithValue("cpf", cpf);
                cmd.Connection = cn;
                dr = cmd.ExecuteReader();
                if (dr.HasRows) { Console.WriteLine("\a\nCPF já em uso"); Console.ReadKey(); }
                else
                {
                    dr.Close();
                    cmd.Parameters.Clear();
                    cmd.CommandText = "INSERT INTO tbl_Usuario (nomeCompleto, cpf, senha) VALUES (@nome, @cpf, @senha)";
                    cmd.Parameters.AddWithValue("nome", nome);
                    cmd.Parameters.AddWithValue("cpf", cpf);
                    cmd.Parameters.AddWithValue("senha", senha);
                    cmd.ExecuteNonQuery();
                    CriarConta(cpf, nome);
                }
            }
            catch (Exception Erro) { Console.WriteLine("\a\nErro: " + Erro.Message); Console.ReadKey(); }
            finally { Desconectar(); }
        }
        //Metodo para criar conta 
        public void CriarConta(string cpf, string nomeCompleto)
        {
            Desconectar();
            StringBuilder sb = new StringBuilder();
            string nome = nomeCompleto.Split(' ')[0];
            var conta = GerarNumeroConta();
            try
            {
                Conectar();
                cmd.Parameters.Clear();
                cmd.CommandText = "INSERT INTO tbl_Conta (cpf, nomeCompleto, nome, saldo, fatura, limite, emprestimo, cartao, conta, numeroCartao)" +
                    "VALUES(@cpf, @nomeCompleto, @nome, 0, 0, 0, 0, 0, @conta, 0)";
                cmd.Parameters.AddWithValue("cpf", cpf);
                cmd.Parameters.AddWithValue("nomeCompleto", nomeCompleto);
                cmd.Parameters.AddWithValue("nome", nome);
                cmd.Parameters.AddWithValue("conta", conta);
                cmd.ExecuteNonQuery();
                variaveisConta vc = new variaveisConta(nomeCompleto, nome, cpf, 0, 0, 0, 0, conta, "0", 0);
            }
            catch (Exception Erro) { Console.WriteLine("\a\nErro: " + Erro.Message); Console.ReadKey(); }
            finally { Desconectar(); }
        }
        //Metodo para criar o numero da conta
        public string GerarNumeroConta()
        {
            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10; i++) { sb.Append(r.Next(0, 9).ToString()); }
            return sb.ToString();
        }
        //Metodo para obter os dados da conta
        public void obterConta(string cpf)
        {
            Desconectar();
            try
            {
                Conectar();
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT * FROM tbl_Conta WHERE cpf = @cpf";
                cmd.Parameters.AddWithValue("cpf", cpf);
                cmd.Connection = cn;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    var nomeCompleto = dr["nomeCompleto"].ToString();
                    var nome = dr["nome"].ToString();
                    var saldo = Convert.ToDecimal(dr["saldo"]);
                    var fatura = Convert.ToDecimal(dr["fatura"]);
                    var limite =  Convert.ToUInt16(dr["limite"]);
                    var emprestimo = Convert.ToDecimal(dr["emprestimo"]);
                    var conta = dr["conta"].ToString();
                    var numeroCartao = dr["numeroCartao"].ToString();
                    var cartao = Convert.ToInt16(dr["cartao"]);
                    variaveisConta vc = new variaveisConta(nomeCompleto, nome, cpf, saldo, fatura, emprestimo, limite, conta, numeroCartao, cartao);
                }

            }
            catch (Exception Erro) { Console.WriteLine("\a\nErro: " + Erro.Message); Console.ReadKey(); }
            finally
            {
                Desconectar();
            }
        }
    }
}

