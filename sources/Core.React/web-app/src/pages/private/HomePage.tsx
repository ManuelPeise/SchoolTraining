import React from 'react';
import { useAuth } from 'src/hooks/useAuth';

const HomePage: React.FC = () => {
  const { logout } = useAuth();

  return (
    <div style={{ padding: '20px' }}>
      <h1>Home Page</h1>

      <button onClick={logout} style={{ padding: '10px 20px', marginTop: '20px' }}>
        Logout
      </button>
    </div>
  );
};

export default HomePage;
