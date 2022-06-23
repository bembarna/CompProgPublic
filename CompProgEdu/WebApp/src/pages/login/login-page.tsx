import React from 'react';
import { Message, Grid, Header, Divider } from 'semantic-ui-react';
import './login-layout.css';
import SignupModal from '../../component/signup-modal/signup-modal';
import { LoginForm } from './LoginForm';

export const LoginPage = () => {
  return (
    <>
      <div style={{ margin: 'auto', height: '100%' }}>
        <Grid relaxed container stackable verticalAlign="middle">
          <Grid.Column id="leftGrid" floated="left" width="10">
            <Header id="pageText" floated="left" as="h1">
              A Better<div id="subPageText">Grading Experience.</div>
            </Header>
          </Grid.Column>
          <Grid.Column id="rightGrid" textAlign="center" width="7">
            <Header id="loginFormHeader" as="h1">
              Log-in to your account
            </Header>
            <LoginForm />
            <Divider
              as="h4"
              className="header"
              horizontal
              style={{ margin: '3em 0em', textTransform: 'uppercase' }}
            >
              <p style={{ color: 'white' }}>Or New Instructor?</p>
            </Divider>
            <Message>
              <Header as="h5" style={{ margin: 5 }}>
                <SignupModal />
              </Header>
            </Message>
          </Grid.Column>
        </Grid>
      </div>
    </>
  );
};
