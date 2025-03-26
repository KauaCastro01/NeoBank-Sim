CREATE DATABASE db_NeoBankSim
USE db_NeoBankSim	

CREATE TABLE tbl_Usuario
(
	idConta INT PRIMARY KEY IDENTITY,
	nomeCompleto VARCHAR(100) NOT NULL,
	cpf VARCHAR(11) NOT NULL UNIQUE,
	senha VARCHAR(30) NOT NULL
)

SELECT * FROM tbl_Usuario
SELECT * FROM tbl_Conta

CREATE TABLE tbl_Conta
(
    cpf VARCHAR(11) PRIMARY KEY,
    nomeCompleto VARCHAR(100) NOT NULL,
    nome VARCHAR(40) NOT NULL,
    saldo DECIMAL(10,2) NOT NULL,
    fatura DECIMAL(10,2),
    limite DECIMAL(10,2),
    emprestimo DECIMAL(10,2),
    cartao INT NOT NULL, 
    conta VARCHAR(20),
	numeroCartao VARCHAR(20),
    FOREIGN KEY (cpf) REFERENCES tbl_Usuario(cpf)
)

ALTER TABLE tbl_Conta
ALTER COLUMN cartao INT

CREATE TABLE tbl_transacoes
(
	id_transacao INT IDENTITY(1,1) PRIMARY KEY, 
    meuCpf VARCHAR(11) NOT NULL, 
    valor DECIMAL(10,2) NOT NULL, 
    nomeDestinatario VARCHAR(100),
    cpfDestinatario VARCHAR(11) NOT NULL, 
    diaEnvio DATETIME NOT NULL DEFAULT GETDATE(), 
    FOREIGN KEY (meuCpf) REFERENCES tbl_Usuario(cpf)
)
DROP TABLE tbl_transacoes
SELECT * FROM tbl_transacoes