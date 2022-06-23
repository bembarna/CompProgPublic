import React from 'react';
import { RouteConfig } from './routing/routes';
import { ToastProvider } from 'react-toast-notifications';
import '../node_modules/react-simple-tree-menu/dist/main.css';

export const App = () => {
  return (
    <>
      <ToastProvider
        placement={'top-center'}
        autoDismiss={true}
        autoDismissTimeout={3000}
      >
        <RouteConfig />
      </ToastProvider>
    </>
  );
};
