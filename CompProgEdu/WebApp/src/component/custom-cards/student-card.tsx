import React from 'react';
import { Card } from 'semantic-ui-react';
import '../custom-cards/custom-cards.css';
import { CustomCardInput, CustomCard } from './custom-cards';

type StudentCardInput = CustomCardInput & {
  header?: string;
  header2?: string;
};
export const StudentCustomCard = (cardProps: StudentCardInput) => {
  return (
    <>
      <CustomCard
        id="customCards"
        cardHeader={cardProps.cardHeader}
        cardDescription={cardProps.cardDescription}
        cardMeta={cardProps.cardMeta}
        cardMetaType={cardProps.cardMetaType}
        image={cardProps.image}
        link={cardProps.link}
        cardContentExtra={cardProps.cardContentExtra}
        linkedinProfile={cardProps.linkedinProfile}
        onClick={cardProps.onClick}
      >
        <Card.Header id="customCardHeader">{cardProps.header}</Card.Header>
        <Card.Header id="customCardHeader2">{cardProps.header2}</Card.Header>
      </CustomCard>
    </>
  );
};
