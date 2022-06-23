import React from 'react';
import { useHistory, useParams } from 'react-router-dom';
import { Container, Header, Tab } from 'semantic-ui-react';
import { StudentAssignmentListing } from '../assignments/assignment-listing';

export const StudentDashboardTabs = () => {
  let { courseId, defaultTab } = useParams() as any;
  const history = useHistory();

  const handleChange = (e: any, { activeIndex }: any) => {
    history.push(`/student/${courseId}/course-dashboard/${activeIndex}`);
  };

  return (
    <div>
      <div style={{ width: '75%', marginLeft: 'auto', marginRight: 'auto' }}>
        <Container style={{ width: '80%' }}>
          <Header style={{ color: 'white', fontSize: '40px' }}>
            Class Info
          </Header>
          <Tab
            defaultActiveIndex={defaultTab}
            onTabChange={handleChange}
            style={{
              backgroundColor: 'white',
              padding: '10px',
              borderRadius: '5px',
            }}
            panes={[
              {
                menuItem: 'Assignments',
                render: () => <StudentAssignmentListing />,
              },
            ]}
          />
        </Container>
      </div>
    </div>
  );
};
