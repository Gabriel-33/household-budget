import React from 'react';
import { NavLink } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';

const Sidebar = () => {
  const { logout } = useAuth();

  const menuItems = [
    { path: '/transacoes', label: 'Transações', icon: '💰' },
    { path: '/pessoas', label: 'Pessoas', icon: '👥' },
    { path: '/categorias', label: 'Categorias', icon: '🏷️' },
    { path: '/relatorios/pessoas', label: 'Relatório Pessoas', icon: '📊' },
    { path: '/relatorios/categorias', label: 'Relatório Categorias', icon: '📈' },
  ];

  return (
    <div className="sidebar">
      <div className="sidebar-header">
        <h1>💰 Sistema de Gastos</h1>
      </div>
      
      <nav className="sidebar-nav">
        <ul>
          {menuItems.map((item) => (
            <li key={item.path}>
              <NavLink 
                to={item.path} 
                className={({ isActive }) => isActive ? 'active' : ''}
              >
                <span className="icon">{item.icon}</span>
                {item.label}
              </NavLink>
            </li>
          ))}
        </ul>
      </nav>
      
      <div className="sidebar-footer">
        <button onClick={logout} className="logout-btn">
          Sair do Sistema
        </button>
      </div>
    </div>
  );
};

export default Sidebar;