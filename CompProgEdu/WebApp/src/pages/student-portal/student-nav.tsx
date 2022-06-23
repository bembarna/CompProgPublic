import React from 'react';
import { Image, Menu, Dropdown } from 'semantic-ui-react';
import { useHistory } from 'react-router-dom';
import { logoutUser } from '../../actions/userAction';

export const StudentNavBar = (props: any) => {
  const history = useHistory();

  const firstName =
    localStorage.getItem('firstName') ?? sessionStorage.getItem('firstName');

  const logoutSubmit = async () => {
    await logoutUser();
    history.go(0);
  };

  return (
    <div>
      <Menu size="massive" inverted borderless style={{ borderRadius: 0 }}>
        <Menu.Item position="left" header>
          <Image
            size="mini"
            src="https://i.imgur.com/mMdNn2h.png"
            style={{ marginRight: '1em' }}
          />
          CompProgEdu
        </Menu.Item>
        <Menu.Item
          as={Dropdown}
          text={`Hi, ${firstName}`}
          children={
            <Dropdown.Menu>
              <Dropdown.Item onClick={logoutSubmit}>Logout</Dropdown.Item>
            </Dropdown.Menu>
          }
        ></Menu.Item>
      </Menu>
    </div>
  );
};
