import React from 'react';
import { Button, Header, Popup } from 'semantic-ui-react';

type deleteButtonProps = {
  buttonText: string;
  event?: any;
  loading?: boolean;
};

export const DeleteButton = (props: deleteButtonProps) => {
  return (
    <Popup
      content="Are you Sure?"
      on="click"
      pinned
      trigger={
        <Button
          type="button"
          floated="left"
          inverted
          color="red"
          style={{ marginRight: '1em' }}
          loading={props.loading}
        >
          {props.buttonText}
        </Button>
      }
    >
      <Header as="h4">Confirm Delete?</Header>
      <Button color="red" onClick={props.event} style={{ marginLeft: '1em' }}>
        Delete
      </Button>
    </Popup>
  );
};
