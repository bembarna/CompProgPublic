import React from 'react';
import { Header, Button, Image, Grid, Icon } from 'semantic-ui-react';
import './home-page.css';
import { Link } from 'react-router-dom';
import { paths } from '../../../routing/paths';

//Testing
export const HomePage = () => {
  return (
    <>
      <div style={{ height: '80%', margin: 'auto' }}>
        <Grid columns="equal" stackable verticalAlign="middle">
          <Grid.Row id="topRow">
            <Grid.Column id="leftHomeGrid">
              <Image
                id="homeImage"
                src="https://i.imgur.com/4tilmjR.png"
                size="big"
                floated="right"
              />
            </Grid.Column>
            <Grid.Column id="rightHomeGrid">
              <Header id="headerOne" as="h1">
                Source Code Grading Tool
              </Header>
              <Header id="headerTwo" as="h3">
                Test Case Based Grading Tool for Students and Professors.
              </Header>
              <Link to={paths.simpleLogin}>
                <Button
                  id="headerButton"
                  color="blue"
                  floated="left"
                  size="huge"
                >
                  Get Started <Icon name="chevron right" size="small" />
                </Button>
              </Link>
            </Grid.Column>
          </Grid.Row>
        </Grid>
      </div>

      <div id="ourPurposeDiv">
        <Grid id="middleGrid" container stackable verticalAlign="middle">
          <Grid.Row>
            <Grid.Column width={8}>
              <Header as="h3" style={{ fontSize: '2em' }}>
                We Connect Students and Professors
              </Header>
              <p style={{ fontSize: '1.33em' }}>
                Our website acts as the middle-man between students source code
                and their professors requirements. CompProgEdu allows professors
                to create test cases, and students to run their code against
                these tests.
              </p>
              <Header as="h3" style={{ fontSize: '2em' }}>
                A Better Grading Experience
              </Header>
              <p style={{ fontSize: '1.33em' }}>
                Our API will handle test case results from our external
                compiler, and determine the correctness of the students code.
                With simple and intuitive results for both the student and
                professor.
              </p>
            </Grid.Column>
            <Grid.Column floated="right" width={6}>
              <Image size="big" src="https://i.imgur.com/bPyBcV4.png" />
            </Grid.Column>
          </Grid.Row>
        </Grid>

        <Grid
          id="bottom Grid"
          relaxed
          stackable
          verticalAlign="middle"
          divided="vertically"
          container
        >
          <Grid.Row columns={3}>
            <Grid.Column id="gridColumns">
              <Header as="h2">Intuitive Interface</Header>
              <p>
                Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean
                commodo ligula eget dolor. Aenean massa strong. Cum sociis
                natoque penatibus et magnis dis parturient montes, nascetur
                ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu,
              </p>
            </Grid.Column>
            <Grid.Column id="gridColumns">
              <Header as="h2">Making Grading Code Easier</Header>
              <p>
                Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean
                commodo ligula eget dolor. Aenean massa strong. Cum sociis
                natoque penatibus et magnis dis parturient montes, nascetur
                ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu,
              </p>
            </Grid.Column>
            <Grid.Column id="gridColumns">
              <Header as="h2">Filling A Void</Header>
              <p>
                Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean
                commodo ligula eget dolor. Aenean massa strong. Cum sociis
                natoque penatibus et magnis dis parturient montes, nascetur
                ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu,
              </p>
            </Grid.Column>
          </Grid.Row>
        </Grid>
      </div>
    </>
  );
};
