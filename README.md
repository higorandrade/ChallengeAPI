# ChallengeAPI

WebAPI para realizar cadastro de empregados e projetos e seus relacionamentos num banco de dados MySQL.

### Pacotes instalados

- Microsoft.AspNetCore.Authentication v2.2
- Microsoft.AspNetCore.Authentication.JwtBearer v5.0.1
- Microsoft.EntityFrameworkCore v5.0.1
- Microsoft.EntityFrameworkCore.Design v5.0.1
- Microsoft.EntityFrameworkCore.Tools v5.0.1
- Pomelo.EntityFrameworkCore.MySql v5.0.1
- Swashbuckle.AspNetCore v5.6.3

### Configuração do Banco de Dados

A string DefaultConnection no arquivo appsettings.json deve ser ajustada

### Utilizando a aplicação

Para utilizar esta WebAPI pela primeira vez, você deve realizar o cadastro de um novo usuário
na base de dados. 

Utilize o endpoint de POST de Usuario informando (username), (senha) e (role) para isso. 

O campo (role) é uma string que deve ser preenchida com 'admin', 'gerente' ou 'projetista'.

#### Autenticação
Obtenha um token inserindo username e senha no endpoint de login.

#### Autorização 
Clique no botão de autorização (Authorize) e insira 'Bearer' + (espaço) + *token*.

#### Permissões
- Os endpoints de cadastro de Usuário e login não requerem autorização
- Para utilizar o endpoint de cadastro de empregado você deve ter função (role) de 'gerente'
- Para utilizar o endpoint de cadastro de projeto você deve ter função (role) de 'gerente' ou 'projetista'
- Para utilizar os endpoints de deleção e alteração de usuário você deve ter função (role) de 'admin'
- Todos os endpoints podem ser acessados pelo 'admin'

#### Cadastro de Empregados

Empregados são cadastrados com um id, nome, sobrenome, telefone, endereço e uma lista de ids de projetos em que participa.
- Caso oculto, id é gerado automaticamente.
- Todos os campos são opcionais, mas, caso informado, telefone deve ser uma string numérica de 10 dígitos.
- Ids de projetos inexistentes são ignorados.

#### Cadastro de Projetos

Projetos são cadastrados com um id, nome, data de criação, data de término, id do gerente e uma lista de ids de empregados que partipam do projeto.
- Caso oculto, id é gerado automaticamente.
- Todos os campos, exceto id do gerente, são opcionais.
- Caso oculta, data de criação assume o valor padrão (01-01-0001 00:00:00).
- Ids de empregados inexistentes são ignorados.
