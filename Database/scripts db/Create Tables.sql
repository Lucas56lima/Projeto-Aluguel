CREATE TABLE CadastroVeiculo(
	veiculoID SERIAL PRIMARY KEY,
	dataVeiculo DATE NOT NULL,
	modeloVeiculo VARCHAR(20) NOT NULL,
	placaVeiculo VARCHAR(7) UNIQUE NOT NULL,
	anoVeiculo INT NOT NULL,
	ativo BOOL DEFAULT(True),	
)

CREATE TABLE Pedidos(
	pedidoID SERIAL PRIMARY KEY,
	dataCriacao DATE NOT NULL,
	status VARCHAR(10) DEFAULT('APROVADO'),
	valorEntrega DECIMAL NOT NULL,
	fk_entregadorID INT,
	ativo BOOL DEFAULT(True)	 
	
)

create table PlanosLocacao(
	planoID SERIAL PRIMARY KEY,
	dias INT NOT NULL,
	valorDiaria DECIMAL NOT NULL
)

create table Mensagens(
	mensagemID SERIAL PRIMARY KEY,
	dataMensagem DATE NOT NULL,
	pedidoID INT,
	mensagem VARCHAR(1000) NOT NULL,
	lida BOOL DEFAULT(false)
)


create table CadastroEntregador(
	entregadorID SERIAL PRIMARY KEY,
	cnpjEntregador VARCHAR(19) UNIQUE NOT NULL,
	nascimentoEntregador DATE NOT NULL,
	numeroCNH VARCHAR(15) UNIQUE NOT NULL,
	tipoCNH VARHCAR(3) NOT NULL,
	locacao BOOL DEFAULT(false),
	arquivoCNH VARCHAR(100),
	ativo BOOL DEFAULT(true)
)

