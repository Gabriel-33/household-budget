import React from 'react';
import { Outlet, Link } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { FiHome, FiUsers, FiTag, FiPieChart, FiUser, FiFolder, FiLogOut } from 'react-icons/fi';

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
            <li>
              <Link to="/transacoes">
                <FiHome />
                <span>Transações</span>
              </Link>
            </li>
            <li>
              <Link to="/pessoas">
                <FiUsers />
                <span>Pessoas</span>
              </Link>
            </li>
            <li className="nav-link">
              <Link to="/categorias">
                <FiTag />
                <span>Categorias</span>
              </Link>
            </li>
            <li className="nav-section">
              <FiPieChart />
              <span>Relatórios</span>
            </li>
            <li>
              <Link to="/relatorios/pessoas">
                <FiUser />
                <span>Por Pessoa</span>
              </Link>
            </li>
            <li>
              <Link to="/relatorios/categorias">
                <FiFolder />
                <span>Por Categoria</span>
              </Link>
            </li>
            <li>
              <Link onClick={logout}>
                <FiLogOut />
                <span>Sair</span>
              </Link>
            </li>
          </ul>
        </nav>
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