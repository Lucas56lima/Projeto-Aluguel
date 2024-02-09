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
	ativo BOOL DEFAULT(True)	 
	
)

create table PlanosLocacao(
	planoID SERIAL PRIMARY KEY,
	dias INT NOT NULL,
	valorDiaria DECIMAL NOT NULL
)

CREATE TABLE 

