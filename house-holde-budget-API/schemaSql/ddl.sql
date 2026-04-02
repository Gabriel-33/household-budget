-- WARNING: This schema is for context only and is not meant to be run.
-- Table order and constraints may not be valid for execution.

CREATE TABLE public.Categorias (
    Id integer NOT NULL DEFAULT nextval('"Categorias_Id_seq"'::regclass),
    Descricao character varying NOT NULL UNIQUE,
    Finalidade character varying NOT NULL CHECK ("Finalidade"::text = ANY (ARRAY['Despesa'::character varying, 'Receita'::character varying, 'Ambas'::character varying]::text[])),
    CONSTRAINT Categorias_pkey PRIMARY KEY (Id)
);
CREATE TABLE public.Pessoas (
    Id integer NOT NULL DEFAULT nextval('"Pessoas_Id_seq"'::regclass),
    Nome character varying NOT NULL,
    Idade integer NOT NULL CHECK ("Idade" >= 0 AND "Idade" <= 150),
    CreatedAt timestamp with time zone DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt timestamp with time zone DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT Pessoas_pkey PRIMARY KEY (Id)
);
CREATE TABLE public.Transacoes (
    Id integer NOT NULL DEFAULT nextval('"Transacoes_Id_seq"'::regclass),
    Descricao character varying NOT NULL,
    Valor numeric NOT NULL CHECK ("Valor" > 0::numeric),
    Tipo character varying NOT NULL CHECK ("Tipo"::text = ANY (ARRAY['Despesa'::character varying, 'Receita'::character varying]::text[])),
    Data timestamp with time zone DEFAULT CURRENT_TIMESTAMP,
    CategoriaId integer NOT NULL,
    PessoaId integer NOT NULL,
    CONSTRAINT Transacoes_pkey PRIMARY KEY (Id),
    CONSTRAINT FK_Transacoes_Categorias_CategoriaId FOREIGN KEY (CategoriaId) REFERENCES public.Categorias(Id),
    CONSTRAINT FK_Transacoes_Pessoas_PessoaId FOREIGN KEY (PessoaId) REFERENCES public.Pessoas(Id)
);
CREATE TABLE public.usuario (
    id_usuario integer NOT NULL DEFAULT nextval('usuario_id_usuario_seq'::regclass),
    nome_usuario character varying NOT NULL,
    email_usuario character varying NOT NULL,
    senha_usuario character varying NOT NULL,
    tipo_usuario integer NOT NULL,
    CONSTRAINT usuario_pkey PRIMARY KEY (id_usuario)
);