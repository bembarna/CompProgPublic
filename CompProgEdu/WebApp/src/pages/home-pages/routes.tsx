import React from 'react';
import { Route, BrowserRouter as Router, Switch } from 'react-router-dom';
import { HomePage } from './home/home-page';
import { LoginPage } from '../login/login-page';
import { paths } from '../../routing/paths';
import { AboutUs } from './about-us/about-us';
import { OurProcess } from './our-process/our-process';
import './routes.css';
import { TestAccountPage } from '../test-accounts/test-account-page';
import { ReactAceTest } from '../../component/ace-editor-test';
import { AuthorizedRoute } from '../../component/auth-wrappers';
import { roles } from '../../enums/roles';
import { HomePageNavBar } from '../home-pages/home-page-nav';

export const HomeRoutes = () => {
  return (
    <div className="home-background">
      <Router>
        <HomePageNavBar />
        <Switch>
          <Route path={paths.home}>
            <HomePage />
          </Route>
          <Route path={paths.ReactAceTest} exact>
            <ReactAceTest />
          </Route>
          <Route path={paths.simpleLogin} exact>
            <AuthorizedRoute noAuth>
              <LoginPage />
            </AuthorizedRoute>
          </Route>
          <Route path={paths.aboutUs} exact>
            <AboutUs />
          </Route>
          <Route path={paths.ourProcess} exact>
            <OurProcess />
          </Route>
          <Route path={paths.testAccount}>
            <AuthorizedRoute role={roles.globalAdmin}>
              <TestAccountPage />
            </AuthorizedRoute>
          </Route>
        </Switch>
      </Router>
    </div>
  );
};
