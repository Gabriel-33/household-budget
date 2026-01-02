import React from 'react';
import { useAuth } from '../../context/AuthContext';

const Header = () => {
  const { user } = useAuth();

  return (
    <header className="header">
      <div className="header-content">
        <div className="user-info">
          <span className="user-avatar">
            {user?.name?.charAt(0) || 'U'}
          </span>
          <div>
            <h3>{user?.name || 'Usuário'}</h3>
            <p className="user-email">{user?.email || ''}</p>
          </div>
        </div>
        
        <div className="header-actions">
          <span className="current-date">
            {new Date().toLocaleDateString('pt-BR')}
          </span>
        </div>
      </div>
    </header>
  );
};

export default Header;