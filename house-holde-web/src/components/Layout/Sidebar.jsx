import { NavLink } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';

const Sidebar = () => {
  const { logout } = useAuth();

  const menuItems = [
    { path: '/transacoes', label: 'Transações'},
    { path: '/pessoas', label: 'Pessoas'},
    { path: '/categorias', label: 'Categorias'},
    { path: '/relatorios/pessoas', label: 'Relatório Pessoas'},
    { path: '/relatorios/categorias', label: 'Relatório Categorias'},
  ];

  return (
    <div className="sidebar">
      <div className="sidebar-header">
        <h1>Sistema de Gastos</h1>
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
      
      <div className="sidebar-nav">
        <button onClick={logout} className="icon">
          Sair
        </button>
      </div>
    </div>
  );
};

export default Sidebar;