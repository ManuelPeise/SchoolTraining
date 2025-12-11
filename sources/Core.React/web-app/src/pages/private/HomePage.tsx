import React from 'react';
import PageLayout from 'src/components/layouts/PageLayout';
import { useAuth } from 'src/hooks/useAuth';

const HomePage: React.FC = () => {
  const { logout } = useAuth();

  return (
    <PageLayout>
      <div style={{ padding: '20px' }}>
        <h1>Home Page</h1>
        <i className="bi bi-house navIcon"></i>
        <button onClick={logout} style={{ padding: '10px 20px', marginTop: '20px' }}>
          Logout
        </button>
      </div>
    </PageLayout>
  );
};

export default HomePage;
