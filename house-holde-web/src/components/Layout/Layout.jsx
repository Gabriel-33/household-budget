import React from 'react';
import { Outlet, Link } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';

const Layout = () => {
  const { user, logout } = useAuth();

  return (
    <div className="app">
      <aside className="sidebar">
        <div className="sidebar-header">
          <h2>Sistema de Gastos</h2>
          <p>Olá, {user?.name || 'Usuário'}!</p>
        </div>
        
        <nav className="sidebar-nav">
          <ul>
            <li><Link to="/transacoes">Transações</Link></li>
            <li><Link to="/pessoas">Pessoas</Link></li>
            <li><Link to="/categorias">Categorias</Link></li>
            <li className="nav-section">Relatórios</li>
            <li><Link to="/relatorios/pessoas">Por Pessoa</Link></li>
            <li><Link to="/relatorios/categorias">Por Categoria</Link></li>
          </ul>
        </nav>
        
        <div className="sidebar-footer">
          <button onClick={logout} className="btn-logout">
            Sair
          </button>
        </div>
      </aside>
      
      <main className="main-content">
        <div className="container">
          <Outlet />
        </div>
      </main>
    </div>
  );
};

export default Layout;