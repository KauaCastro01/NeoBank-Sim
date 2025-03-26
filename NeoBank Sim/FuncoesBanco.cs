using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoBank_Sim
{
    class FuncoesBanco
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
            try { cn.Close(); }
            catch (Exception ex) { throw new Exception("\a\nErro ao conectar ao banco de dados: " + ex.Message); }
        }
        //Metodo para executar comandos SQL
        //Metodo para transferir dinheiro para o proprio banco
        public bool prorpioBanco(string cpf, decimal saldo, string meuCpf, int i)
        {
            try
            {
                Conectar();
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT * FROM tbl_Usuario WHERE cpf = @cpf";
                cmd.Parameters.AddWithValue("cpf", cpf);
                cmd.Connection = cn;
                dr = cmd.ExecuteReader();
                if (dr.HasRows || i == 2)
                {
                    dr.Close();
                    Conectar();
                    cmd.Parameters.Clear();
                    cmd.CommandText = "UPDATE tbl_Conta SET saldo = saldo - @saldo WHERE cpf = @meuCpf";
                    cmd.Parameters.AddWithValue("saldo", saldo);
                    cmd.Parameters.AddWithValue("meuCpf", meuCpf);
                    cmd.Connection = cn;
                    cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "UPDATE tbl_Conta SET saldo = saldo + @saldo WHERE cpf = @cpf";
                        cmd.Parameters.AddWithValue("saldo", saldo);
                        cmd.Parameters.AddWithValue("cpf", cpf);
                        cmd.Connection = cn;
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                else { Console.WriteLine("\a\nUsuário não encontrado ou usuário utiliza outro banco"); Console.ReadKey(); return false; }
            }
            catch (Exception Erro) { Console.WriteLine("\a\nErro: " + Erro.Message); Console.ReadKey(); return false; }
            finally { Desconectar(); }
        }
         //Metodo para salvar o comprovante de transação dele
        public void criarComprovante(string meuCpf, decimal valor, string cpfDestinatario)
        {
            try
            {
                Conectar();
                cmd.Parameters.Clear();
                cmd.CommandText = "INSERT INTO tbl_transacoes (meuCpf, valor, nomeDestinatario, cpfDestinatario, diaEnvio) " +
                  "VALUES (@meuCpf, @valor, ' ', @cpfDestinatario, GETDATE())";
                cmd.Parameters.AddWithValue("@meuCpf", meuCpf);
                cmd.Parameters.AddWithValue("@valor", valor);
                cmd.Parameters.AddWithValue("@cpfDestinatario", cpfDestinatario);
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();

            }
            catch (Exception Erro) { Console.WriteLine("\a\nErro: " + Erro.Message); Console.ReadKey(); }
            finally { Desconectar(); }
        }
        //Ver todas as transferencias existente
        public void verHistorico(string cpf)
        {
            Conectar();
            cmd.Parameters.Clear();
            cmd.CommandText = "SELECT * FROM tbl_transacoes WHERE meuCpf = @cpf";
            cmd.Parameters.AddWithValue("cpf", cpf);
            cmd.Connection = cn;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var cpfDestinatario = dr["cpfDestinatario"];
                var valor = Convert.ToDecimal(dr["valor"]);
                var data = Convert.ToDateTime(dr["diaEnvio"]);

                Console.WriteLine("Quem recebeu: " + cpfDestinatario);
                Console.WriteLine("Valor enviado: " + valor);
                Console.WriteLine("Data: " + data);
                Console.WriteLine("--------------------");
            }
        }
        //Solicitarr emprestimo
        public void SolicitarEmprestimo(string cpf, decimal valor, decimal valorJuros)
        {
            try
            {
                Conectar();
                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE tbl_Conta SET saldo = saldo + @valor, emprestimo = @valorJuros WHERE cpf = @cpf";
                cmd.Parameters.AddWithValue("cpf", cpf);
                cmd.Parameters.AddWithValue("valor", valor);
                cmd.Parameters.AddWithValue("valorJuros", valorJuros);
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();
                Console.WriteLine("Emprestimo concluido com sucesso!");
                Console.ReadKey();
            }
            catch (Exception Erro) { Console.WriteLine("\a\nErro: " + Erro.Message); Console.ReadKey();  }
            finally { Desconectar(); }
        }
        //pagar emprestimo em aberto
        public void PagarEmprestimo(string cpf, decimal saldo, decimal valorPago)
        {
            try
            {
                Conectar();
                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE tbl_Conta SET saldo = saldo - @saldo, emprestimo = emprestimo - @valorPago WHERE cpf = @cpf";
                cmd.Parameters.AddWithValue("cpf", cpf);
                cmd.Parameters.AddWithValue("saldo", saldo);
                cmd.Parameters.AddWithValue("valorPago", valorPago);
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();
                Console.WriteLine("Pagamento concluido!");
                Console.ReadKey();
            }
            catch (Exception Erro) { Console.WriteLine("\a\nErro: " + Erro.Message); Console.ReadKey(); }
            finally { Desconectar(); }
        }
        //Registar numeros do cartão
        public int SalvarCartão(string cpf, string numeroCartao)
        {
            try
            {
                Conectar();
                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE tbl_Conta SET cartao = cartao + 1, numeroCartao = @numeroCartao, limite = 200  WHERE cpf = @cpf";
                cmd.Parameters.AddWithValue("cpf", cpf);
                cmd.Parameters.AddWithValue("numeroCartao", numeroCartao);
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();
                Console.WriteLine("\nCartão Criado");
                Console.ReadKey();
                return 1;
            }
            catch (Exception Erro) { Console.WriteLine("\a\nErro: " + Erro.Message); Console.ReadKey(); return 0; }
            finally { Desconectar(); }
        }
        //Exibir numeros do cartao
        public bool ExibirCartao(string cpf, string senha, string numeroCartao)
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
                if (dr.HasRows) { Console.WriteLine("Número do cartão: " + numeroCartao); Console.ReadKey(); return true;  }
                else { Console.WriteLine("\a\nUsuário não encontrado!"); Console.ReadKey(); return false; }
            }
            catch (Exception Erro) { Console.WriteLine("\a\nErro: " + Erro.Message); Console.ReadKey(); return false; }
            finally { Desconectar(); }
        }
        //Deletar o cartao
        public void DeletarCartao(string cpf)
        {
            try
            {
                Conectar();
                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE tbl_Conta SET cartao = 0, numeroCartao = 0  WHERE cpf = @cpf";
                cmd.Parameters.AddWithValue("cpf", cpf);
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();
                Console.WriteLine("\nCartão Deletado");
                Console.ReadKey();
            }
            catch (Exception Erro) { Console.WriteLine("\a\nErro: " + Erro.Message); Console.ReadKey(); }
            finally { Desconectar(); }
        }
        //aumentar e diminuit o limite do credito
        public void AumentarLimite(string cpf, decimal valor, int i)
        {
            try
            {
                Conectar();
                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE tbl_Conta SET limite = @valor WHERE cpf = @cpf";
                cmd.Parameters.AddWithValue("valor", valor);
                cmd.Parameters.AddWithValue("cpf", cpf);
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();
                if(i == 1) { Console.WriteLine("Limite aumentado"); }
                if (i == 2) { Console.WriteLine("Limite diminuido"); }
                Console.ReadKey();
            }
            catch (Exception Erro) { Console.WriteLine("\a\nErro: " + Erro.Message); Console.ReadKey(); }
            finally { Desconectar(); }
        }
        //pesquisar o usuario
        public bool PesquisarUsuario(string cpf, string senha)
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
                if (dr.HasRows) { return true; }
                else { Console.WriteLine("\a\nUsuário não encontrado!"); Console.ReadKey(); return false; }
            }
            catch (Exception Erro) { Console.WriteLine("\a\nErro: " + Erro.Message); Console.ReadKey(); return false; }
            finally { Desconectar(); }
        }
        //Mudar a senha
        public void MudarSenha(string cpf, string senha)
        {
            try
            {
                Conectar();
                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE tbl_Usuario SET senha = @senha WHERE cpf = @cpf";
                cmd.Parameters.AddWithValue("cpf", cpf);
                cmd.Parameters.AddWithValue("senha", senha);
                cmd.Connection = cn;
                cmd.ExecuteReader();
                Console.WriteLine("Senha atualizada");
                Console.ReadKey();
            }
            catch (Exception Erro) { Console.WriteLine("\a\nErro: " + Erro.Message); Console.ReadKey(); }
            finally { Desconectar(); }
        }

    }
}
