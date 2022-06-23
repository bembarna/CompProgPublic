import React from 'react';
import { Card } from 'semantic-ui-react';
import '../custom-cards/custom-cards.css';
import { CustomCardInput, CustomCard } from './custom-cards';

type InstructorCardInput = CustomCardInput & {
  instructorCardHeader?: string;
  instructorCardHeader2?: string;
};
export const InstructorCustomCard = (cardProps: InstructorCardInput) => {
  return (
    <>
      <CustomCard
        id="customCards"
        cardHeader={cardProps.cardHeader}
        instructorCardHeader={cardProps.instructorCardHeader}
        instructorCardHeader2={cardProps.instructorCardHeader2}
        cardDescription={cardProps.cardDescription}
        cardMeta={cardProps.cardMeta}
        cardMetaType={cardProps.cardMetaType}
        image={cardProps.image}
        link={cardProps.link}
        cardContentExtra={cardProps.cardContentExtra}
        linkedinProfile={cardProps.linkedinProfile}
        onClick={cardProps.onClick}
      >
        <Card.Header id="customCardHeader">
          {cardProps.instructorCardHeader}
        </Card.Header>
        <Card.Header id="customCardHeader2">
          {cardProps.instructorCardHeader2}
        </Card.Header>
      </CustomCard>
    </>
  );
};
