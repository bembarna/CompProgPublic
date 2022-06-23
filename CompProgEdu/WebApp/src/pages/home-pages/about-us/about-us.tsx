import React from 'react';
import { Header, Card, Grid, Container, Image, Label } from 'semantic-ui-react';
import './about-us.css';
import { LinkedinCard } from '../../../component/custom-cards/linkedin-card';

export const AboutUs = () => {
  return (
    <>
      <div style={{ height: '30%', margin: 'auto' }}>
        <Grid verticalAlign="middle">
          <Grid.Row>
            <Grid.Column>
              <Header id="aboutUsHeader" as="h1">
                About CompProgEdu
              </Header>
            </Grid.Column>
          </Grid.Row>
        </Grid>
      </div>
      <div id="aboutUsDiv">
        <Container id="aboutUsContainer" text>
          <Header id="aboutUsContainerHeader" as="h2">
            A Void That Needed Filling
            <div id="subAboutContainerHeader">
              - Clinton Walker, <span id="authorSubTitleText">Ph.D*</span>
              <Label
                as="a"
                href="https://www.linkedin.com/in/clintonjwalker/"
                content="View My Linkedin"
                icon="linkedin"
                color="blue"
              />
            </div>
          </Header>

          <p>
            <LeftImage />
            Computer Science has accelerated in growth as a university major and
            research field. The past twenty years have shown a massive in gain
            in the number of software engineering students coming out of
            universities. Freshman classes are getting significantly bigger as
            first year enrollment in the major multiplies. In smaller
            universities, instructors may not receive aide in organizing and
            grading assignments for one of these large introductory courses. The
            goal of CompProgEdu is to expedite and improve the assignment
            grading and feedback process.
          </p>
          <p>
            Solutions similar to CompProgEdu exist, but the unique goal of
            CompProgEdu is a modular application that provides flexible grading
            solutions specifically for Computer Science professors and
            instructors. Students can receive instant feedback on their
            submissions for an assignment, and instructors can greatly reduce
            grading time in a semester. This benefits both parties immensely.
            With less time spent on grading, more effort can be devoted to
            instruction and helping students. Students can incrementally check
            and improve their assignments while they continue to work on them.
            The final goal of CompProgEdu is to take this single piece of the
            Computer Science academic dynamic and improve it, making the entire
            major itself better.
          </p>
        </Container>

        <Grid centered>
          <Container id="ourTeamContainer">
            <Header id="teamHeader" textAlign="center">
              Meet Our Talented Dev Team
            </Header>
            <TeamCardList />
          </Container>
        </Grid>
      </div>
    </>
  );
};

const TeamCardList = () => {
  return (
    <>
      <Card.Group relaxed stackable>
        <LinkedinCard
          cardHeader="Calvin Wood"
          cardMeta="Front End"
          image="https://i.imgur.com/ogZykVj.png"
          cardContentExtra="Linkedin Profile"
          linkedinProfile="https://www.linkedin.com/in/calvin-wood-865340171/"
        />
        <LinkedinCard
          cardHeader="Robert Johnston"
          cardMeta="Full Stack"
          image="https://i.imgur.com/7xolQX9.jpg"
          cardContentExtra="Linkedin Profile"
          linkedinProfile="https://www.linkedin.com/in/rjosephjohnston/"
        />
        <LinkedinCard
          cardHeader="Aidan Higginbotham"
          cardMeta="Full Stack"
          image="https://i.imgur.com/IGHRTAV.png"
          cardContentExtra="Linkedin Profile"
          linkedinProfile="https://www.linkedin.com/in/aidan-higginbotham-624067194/"
        />
        <LinkedinCard
          cardHeader="Brian Lark"
          cardMeta="Back End"
          image="https://i.imgur.com/dS8Fiqy.jpg"
          cardContentExtra="Linkedin Profile"
          linkedinProfile="https://www.linkedin.com/in/brian-lark-a320a4162/"
        />
        <LinkedinCard
          cardHeader="Lauren Pace"
          cardMeta="Back End"
          image="https://i.imgur.com/vTVEApY.jpg"
          cardContentExtra="Linkedin Profile"
          linkedinProfile="https://www.linkedin.com/in/lauren-pace-87b0651a3/"
        />
        <LinkedinCard
          cardHeader="Zachary Whiddon"
          cardMeta="Back End"
          image="https://i.imgur.com/dKh1gpy.jpg"
          cardContentExtra="Linkedin Profile"
          linkedinProfile="https://www.linkedin.com/in/zachary-whiddon-121a56173/"
        />
      </Card.Group>
    </>
  );
};

const LeftImage = () => {
  return (
    <>
      <Image
        circular
        floated="left"
        size="small"
        src="https://i.imgur.com/zOfagAF.jpg"
        style={{ margin: '1em 1em 1em 0em' }}
      />
    </>
  );
};
