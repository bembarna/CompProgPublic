import React from 'react';
import { Image, Menu, Dropdown } from 'semantic-ui-react';
import { NavLink, useHistory } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { paths } from '../../routing/paths';
import { logoutUser } from '../../actions/userAction';
import { AuthorizedComponent } from '../../component/auth-wrappers';

export const HomePageNavBar = (props: any) => {

  const history = useHistory();

  const firstName = localStorage.getItem("firstName") ?? sessionStorage.getItem("firstName");

  const logoutSubmit = async () => {
    await logoutUser();
    history.go(0);
  };

  return (
    //THIS MAIN NAV BAR WILL BE THE NAV BAR THAT HAS TAB ACCESSIBLE FOR BOTH LOGGED IN USERS AND LOGGED OUT USERS
    <div>
      <Menu size="massive" fixed="top" inverted borderless>
        <Menu.Item position="left" as={Link} to={paths.home} header>
          <Image
            size="mini"
            src="https://i.imgur.com/mMdNn2h.png"
            style={{ marginRight: '1em' }}
          />
          CompProgEdu
        </Menu.Item>
        <Menu.Item as={NavLink} to={paths.home}>
          Home
        </Menu.Item>
        <Menu.Item as={NavLink} to={paths.ReactAceTest}>
          Code Playground
        </Menu.Item>
        <Menu.Item as={NavLink} to={paths.aboutUs}>
          About Us
        </Menu.Item>
        {/* THINGS PEOPLE WHO ARE NOT SIGNED IN SEE */}
        <AuthorizedComponent notAuthenticated>
          <Menu.Item as={NavLink} to={paths.simpleLogin}>
            Login
          </Menu.Item>
        </AuthorizedComponent>
        {/* THINGS PEOPLE WHO ARE SIGNED IN SEE */}
        <AuthorizedComponent>
          {/* <Menu.Item as={NavLink} to={paths.testAccount}>
            Test Accounts
          </Menu.Item> */}
          <Menu.Item
            as={Dropdown}
            text={`Hi, ${firstName}`}
            children={
              <Dropdown.Menu>
                <Dropdown.Item
                  onClick={logoutSubmit}

                >
                  Logout
                </Dropdown.Item>
              </Dropdown.Menu>
            }
          ></Menu.Item>
        </AuthorizedComponent>
      </Menu>
    </div>
  );
};

