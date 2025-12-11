import React from 'react';
import PageLayout from 'src/components/layouts/PageLayout';

const HomePage: React.FC = () => {
  return (
    <PageLayout isLoading={false}>
      <div style={{ padding: '20px' }}>
        <h1>Home Page</h1>
      </div>
    </PageLayout>
  );
};

export default HomePage;
