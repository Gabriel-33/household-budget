import React from 'react';

const PessoaCard = ({ pessoa, onDelete }) => {
  return (
    <div className="pessoa-card">
      <div className="pessoa-header">
        <div className="pessoa-info">
          <h3>{pessoa.nome}</h3>
          <p className="pessoa-id">ID: {pessoa.id}</p>
        </div>
        <div className="pessoa-actions">
          <button 
            onClick={() => onDelete(pessoa.id)}
            className="btn-delete"
            title="Excluir pessoa"
          >
            Excluir
          </button>
        </div>
      </div>
      
      <div className="pessoa-detalhes">
        <div className="detalhe-item">
          <span className="detalhe-label">Idade:</span>
          <span className="detalhe-valor">{pessoa.idade} anos</span>
        </div>
        <div className="detalhe-item">
          <span className="detalhe-label">Status:</span>
          <span className={`detalhe-status ${pessoa.idade < 18 ? 'menor' : 'adulto'}`}>
            {pessoa.idade < 18 ? 'Menor de idade' : 'Adulto'}
          </span>
        </div>
      </div>
      
      {pessoa.idade < 18 && (
        <div className="pessoa-alerta">
          <span className="alerta-icon">⚠️</span>
          <span className="alerta-text">
            Esta pessoa só pode ter transações do tipo DESPESA
          </span>
        </div>
      )}
    </div>
  );
};

export default PessoaCard;