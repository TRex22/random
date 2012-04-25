//what i need (necesary)
#include <iostream>
#include <cmath>
#include <time.h>
#include "console.h"
using namespace std;
namespace con = JadedHoboConsole;


//variables
int inputnum;
long int taken, given;
int version = 0;
int subversion = 4;

//classes
class player
{
  public:
    int level;  //using it on seperate lines allows easy commenting of what each var is
    int experience;
    int stat;
    long int health;
    long int maxhealth;
    long int atk;
	long int maxAtk;
	long int maxDef;
    long int def;
	int spirit;
	int fatigue;
    int bank;
	int needed;
	long int addAtk;
    long int addDef;
	int deaths;
	//item variables
	int knife, sword, spear;
	int shield, chestplate, helm;
	int totem_a, totem_b, totem_c;
  
    player()  // this is called a constructor.  It sets up the object when it is created.
    {
 //don't need to check if level=0, cause we've just created the object.
        level = 1;
        experience = 0;
        health = 250;
        maxhealth = 250;
		spirit = 0;
		fatigue = 0;
        atk = 15;
		maxAtk = 15;
        def = 15;
		maxDef = 15;
        bank = 1000;
		stat = 0;
		needed = 20;
		addAtk = 0;
		addDef = 0;
		deaths = 0;

		//items
		knife = 0;
		sword = 0;
		spear = 0;
		shield = 0;
		chestplate = 0;
		helm =0;
		totem_a = 0;
		totem_b = 0;
		totem_c = 0;

    };

  private:
 // can't think of anything that should be private.  It is generally good programming practice to put variables private
 // and have public functions like setDef().  Shouldn't hurt to have an empty private, though.
}; //end of class declaration

class monster
{
  public:
    int atk;
    int def;
    int hp;
    int maxhp;
    int level;

  
    monster()
    {
      atk=15;
      def=15;
      hp= 150;
      maxhp=150;
      level=1;
    };
};

class item
{
  public:
    int atk;
    int def;
    int type; //1 = weap, 2 = arm, 3 = misc
  
    item()
    {
      atk=0;
      def=0;
      type= 0;
    };
};
//allow use of functions
int menu(player &thisPlayer, monster &thisMonster,
		 item &wep1, item &wep2, item &wep3,
		 item &arm1, item &arm2, item &arm3);
int attack(player &thisPlayer, monster &thisMonster);
int playerInfo(player &thisPlayer, monster &thisMonster);
int playerLvl(player &thisPlayer, monster &thisMonster);
int equips(player &thisPlayer,
		 item &wep1, item &wep2, item &wep3,
		 item &arm1, item &arm2, item &arm3);
int atkUp(player &thisPlayer);
int buy(player &thisPlayer);
int defUp(player &thisPlayer);
int spirit(player &thisPlayer);
int heal(player &thisPlayer);
int healsp(player &thisPlayer);
int drawMenu(player &thisPlayer);
int drawAtk();
int drawSpirit();
int give();
int take();

//sleep function
void sleep(unsigned int mseconds)
{
clock_t goal = mseconds + clock();
while (goal > clock());
} 

//---------------------------------------------------------------------------------
//main
int main()
{
    player User;
    monster Enemy;

	//initialisation
	cout<<"Preparing Weapons...\n";
	sleep(100);
	//weapons
	item knife;
	knife.atk = 10;
	knife.type = 1;

	item sword;
	sword.atk = 30;
	sword.type = 1;

	item spear;
	spear.atk = 35;
	spear.type = 1;
	
	cout<<"Preparing Armour...\n";
	sleep(100);
	//armour
	item shield;
	shield.def = 15;
	shield.type = 2;

	item chestplate;
	chestplate.def = 25;
	chestplate.type = 2;

	item helm;
	helm.def = 30;
	helm.type = 2;

	cout<<"Preparing Items...\n";
	sleep(100);
	//items
	item totem_a;
	totem_a.type = 3;
	item totem_b;
	totem_b.type = 3;
	item totem_c;
	totem_c.type = 3;

	sleep(100);
	cout<<"\nNotice: "<<con::fg_red<<"Please avoid inputting letters, they dont work, and crash URPG.\n";
	sleep(5000);
    menu(User,Enemy,knife,sword,spear,shield,chestplate,helm);
	cout<<"\n\nThank you for playing!";
	sleep(5000);
	return 0;
};

//---------------------------------------------------------------------------------
//functions and code --------------------------------------------------------------
//---------------------------------------------------------------------------------
//---------------------------------------------------------------------------------
//menu ----------------------------------------------------------------------------
//---------------------------------------------------------------------------------


int menu(player &thisPlayer, monster &thisMonster,
		 item &wep1, item &wep2, item &wep3,
		 item &arm1, item &arm2, item &arm3)
{
    while (1==1) {
    int inputnum = 0;
	thisPlayer.addAtk = (thisPlayer.knife * wep1.atk) + (thisPlayer.sword * wep2.atk) + (thisPlayer.spear * wep3.atk);
	thisPlayer.addDef = (thisPlayer.shield * arm1.def) + (thisPlayer.chestplate * arm2.def) + (thisPlayer.helm * arm2.def);
	con::clr( cout );
    //main menu
    //played from here, the whole menu.
    cout<<con::fg_red<<"\n\nUnlimitedRPG v"<<version<<"."<<subversion<<"\n";
    cout<<"--------\n";
	drawMenu(thisPlayer);
    cout<<con::fg_white<<"Level: "<< thisPlayer.level <<"\n";
    cout<<"Health "<< thisPlayer.health <<"/"<< thisPlayer.maxhealth <<"\n";
    cout<<"Enemy Level: "<<thisMonster.level<<"\n";
    cout<<"Enemy HP: "<<thisMonster.hp<<"/"<<thisMonster.maxhp<<"\n";
    //menu items
    cout<<con::fg_blue<<"1)"<<con::fg_white<<" Player Info\n";
	cout<<con::fg_blue<<"2)"<<con::fg_white<<" Equips\n";
	cout<<con::fg_blue<<"3)"<<con::fg_white<<" Attack the Enemy\n";
	cout<<con::fg_blue<<"4)"<<con::fg_white<<" Spirit Power ("<<thisPlayer.spirit<<"s "<<thisPlayer.fatigue<<"f)\n";
	cout<<con::fg_blue<<"5)"<<con::fg_white<<" Heal Spirit (Cost: "<<(thisPlayer.level * 900)<<")\n";
	cout<<con::fg_blue<<"6)"<<con::fg_white<<" Heal Body (Cost: 500)\n";


	//level + extra special items
	if (thisPlayer.stat > 0) {
		cout<<con::fg_blue<<"7)"<<con::fg_white<<" Increase Attack Power\n";
		cout<<con::fg_blue<<"8)"<<con::fg_white<<" Increase Defence Amount\n";
	}
	if (thisPlayer.needed < thisPlayer.experience) {
		cout<<con::fg_blue<<"9)"<<con::fg_white<<" Level\n";
	}
	cout<<con::fg_blue<<"10 or other)"<<con::fg_white<<" Quit\n";


    cout<<"Choice: ";
    cin>>inputnum;
	if (inputnum == 99) inputnum -= 10;
	if ((thisPlayer.needed > thisPlayer.experience) && (inputnum == 9)) {
		inputnum = 99;
	}
	if (cin.fail()) { cin.clear(); inputnum = 100; }
    switch (inputnum)
    {
	default: sleep(1000); return 0; break;
      case 1: sleep(1000); playerInfo(thisPlayer, thisMonster); break;
	  case 2: sleep(1000); equips(thisPlayer,wep1,wep2,wep3,arm1,arm2,arm3); sleep(1000); break;
	  case 3: sleep(1000); attack(thisPlayer, thisMonster); break;
	  case 4: sleep(1000); spirit(thisPlayer); break;
	  case 5: sleep(1000); healsp(thisPlayer); sleep(1000); break;
	  case 6: sleep(1000); heal(thisPlayer); sleep(1000); break;
	  case 7: sleep(1000); atkUp(thisPlayer); sleep(1000); break;
	  case 8: sleep(1000); defUp(thisPlayer); sleep(1000); break;
	  case 9: sleep(1000); playerLvl(thisPlayer, thisMonster); break;
	  case 99:  sleep(2000); con::clr( cout ); cout<<"\n\nYou cant Level yet..."; sleep(2000); break;
	  case 100: sleep(1000); break;
	  case 10: sleep(3000); return 0; break;

    };
	};
};

//--------------------------------------------------------------------------------------------------------------
// Attack Functions --------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------------------

int attack(player &thisPlayer, monster &thisMonster)
{
	con::clr( cout );
	cout<<con::fg_red<<"\nYou chose to attack the monster!\n";
	taken = int((thisMonster.atk-(thisPlayer.def - thisMonster.atk * 2)*rand()/RAND_MAX)+0.5) - thisPlayer.addDef;
	given = int((thisPlayer.atk-(thisMonster.def - thisPlayer.atk * 2)*rand()/RAND_MAX)+0.5) + thisPlayer.addAtk;
	if (taken <= 0) taken = 1;
	if (given <= 0) given = 1;
	thisPlayer.health -= taken;
	thisMonster.hp -= given;
	give();
	cout<<con::fg_white<<"You deal "<<given<<" damage.\n";
	sleep(1000);
	take();
	cout<<con::fg_white<<"You take "<<taken<<" damage.\n";
	sleep(1000);
	if (thisPlayer.fatigue > 0) {
		if (thisPlayer.spirit > 0) {
			thisPlayer.spirit -= 1;
			thisPlayer.fatigue += 1;
		}
	}
	if ((thisPlayer.health <= 0) && (thisMonster.hp > 0)) {
			cout<<"You have died! what a shame!\n";
			thisPlayer.health = thisPlayer.maxhealth;
			thisMonster.level += 1;
			thisMonster.maxhp += 10;
			thisMonster.hp = thisMonster.maxhp;
			thisPlayer.deaths += 1;
	} else if ((thisPlayer.health > 0) && (thisMonster.hp <= 0)) {
			cout<<"You have conquered the monster! Well done!\n";
			thisPlayer.maxhealth = (thisPlayer.level * 30) + thisPlayer.maxhealth;
			thisPlayer.health = thisPlayer.maxhealth;
			thisPlayer.experience += thisMonster.level * 25;
			thisPlayer.bank += thisMonster.maxhp;
			thisMonster.level += 1;
			thisMonster.maxhp = thisMonster.maxhp + (thisMonster.level * 25);
			thisMonster.hp = thisMonster.maxhp;
			thisMonster.atk += 4;
			thisMonster.def += 4;
			thisPlayer.atk += 3;
			thisPlayer.def += 3;
			thisPlayer.spirit += 1;

	} else if ((thisPlayer.health <= 0) && (thisMonster.hp <= 0)) {
			cout<<"WOW! A double KO!\n";
			cout<<"You and your Enemy have been revived, so please be more carefull next time!\n";
			thisPlayer.health = thisPlayer.maxhealth;
			thisMonster.hp = thisMonster.maxhp;
			thisPlayer.deaths += 1;
	};
	if ((thisPlayer.spirit == 0) && (thisPlayer.fatigue != 0)) {
		sleep(1000);
		cout<<"\nYour fatigue gets to you and you reach your limit!\n";
		thisPlayer.atk = thisPlayer.maxAtk;
		thisPlayer.def = thisPlayer.maxDef;

	}
	sleep(1000);
	return 0;

}

//--------------------------------------------------------------------------------------------------------------
//Action Functions (eg. heal/buy equips) -----------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------------------

int playerLvl(player &thisPlayer, monster &thisMonster)
{
	con::clr( cout );
	cout<<con::fg_red<<"\n\nnote: Levelling increases your AND the monsters power!\n";
	cout<<"Please choose what to level:\n";
	cout<<con::fg_blue<<"1)"<<con::fg_white<<" Player Level\n";
	cout<<con::fg_blue<<"2)"<<con::fg_white<<" Spirit Power\n";
	cout<<con::fg_blue<<"3)"<<con::fg_white<<" Attack and Defence\n";
	cout<<con::fg_blue<<"Other)"<<con::fg_white<<" Return to menu without levelling\n";
	cout<<"Choice: ";
	cin>>inputnum;
	if (cin.fail()) { cin.clear(); inputnum = 100; }
    switch (inputnum)
    {
	default: return 0; break;
	case 1:  {
			thisPlayer.level += 1;
			thisPlayer.maxhealth += 10 * thisPlayer.level;
			thisMonster.maxhp += 11 * thisMonster.level;
			thisPlayer.health = thisPlayer.maxhealth;
			thisMonster.hp = thisMonster.maxhp;
			thisPlayer.experience -= thisPlayer.needed;
			thisPlayer.needed += 10;
			thisPlayer.stat += 3;
			thisMonster.atk += 20;
			thisMonster.def += 20;
			 }; break;
	case 2:  {
			thisPlayer.spirit += 2;
			thisMonster.atk += 11 * thisPlayer.level;
			thisMonster.def += 11 * thisPlayer.level;
			thisMonster.maxhp += 11 * thisMonster.level;
			thisPlayer.experience -= thisPlayer.needed;
			thisPlayer.needed += 20;
			  }; break;
	case 3: {
			thisPlayer.atk += 10*thisPlayer.level;
			thisPlayer.def += 10*thisPlayer.level;
			thisMonster.atk += 11*thisPlayer.level;
			thisMonster.def += 11*thisPlayer.level;
			thisPlayer.experience -= thisPlayer.needed;
			thisPlayer.needed += 30;
			}; break;
	};
	return 0;
}

//--------------------------------------------------------------------------------------------------------------

int atkUp(player &thisPlayer)
{
		con::clr( cout );
	if (thisPlayer.stat > 0) {
	thisPlayer.stat -= 1;
	thisPlayer.atk += 20;
	thisPlayer.maxAtk += 20;
	cout<<con::fg_white<<"\n\nYour attack power has increased!";
	} else {
	cout<<con::fg_red<<"\n\nYou cannot increase your attack at the moment!";
	}
	return 0;
}

//--------------------------------------------------------------------------------------------------------------

int defUp(player &thisPlayer)
{
		con::clr( cout );
	if (thisPlayer.stat > 0) {
	thisPlayer.stat -= 1;
	thisPlayer.def += 20;
	thisPlayer.maxDef += 20;
	cout<<con::fg_white<<"\n\nYour defence power has increased!";
	} else {
	cout<<con::fg_red<<"\n\nYou cannot increase your defence at the moment!";
	}
	return 0;
}

//--------------------------------------------------------------------------------------------------------------

int spirit(player &thisPlayer)
{
		con::clr( cout );
	if (thisPlayer.spirit > 0) {
		thisPlayer.spirit -= 1;
		thisPlayer.fatigue += 1;
		drawSpirit();
		cout<<con::fg_white<<"\n\nYou feel more powerfull!";
		if (thisPlayer.spirit == 0) {
			cout<<"\nYou realise you will have to rest after this attack!";
		} else if (thisPlayer.fatigue > thisPlayer.spirit) {
			sleep(1000);
			cout<<"\nYou feel a bit fatigued";
		}
		thisPlayer.atk = thisPlayer.atk + int(thisPlayer.atk * (0.1 * thisPlayer.level) + 0.5);
		thisPlayer.def = 0;
	} else {
		cout<<"\n\nYour spirit power has been depleted!";
		sleep(1000);
	}
	return 0;
}

//--------------------------------------------------------------------------------------------------------------

int healsp(player &thisPlayer)
{
		con::clr( cout );
	if ((thisPlayer.spirit > 0) || (thisPlayer.fatigue == 0)) {
		cout<<con::fg_white<<"You dont require healing!\n";
		cout<<con::fg_white<<"This Could be due to still having spirit power, or not being fatigued!";
	} else {
		if (thisPlayer.bank >= (100*thisPlayer.level)) {
			thisPlayer.health = thisPlayer.maxhealth;
			thisPlayer.spirit = thisPlayer.fatigue;
			thisPlayer.atk = thisPlayer.maxAtk;
			thisPlayer.def = thisPlayer.maxDef;
			thisPlayer.fatigue = 0;
			cout<<"You feel refreshed!";
		} else {
			cout<<"You dont have enough cash!";
		}
	}
	return 0;
}

//--------------------------------------------------------------------------------------------------------------

int heal(player &thisPlayer)
{
		con::clr( cout );
	if (thisPlayer.health == thisPlayer.maxhealth) {
		cout<<con::fg_white<<"You dont need a heal!\n";
	}else if (thisPlayer.bank > 500) {
		cout<<"You feel refreshed!\n";
		thisPlayer.health = thisPlayer.maxhealth;
	}
	sleep(1000);
	return 0;
}

//--------------------------------------------------------------------------------------------------------------

int buy(player &thisPlayer)
{
	while(1 == 1) {
		con::clr( cout );
		cout<<con::fg_red<<"Bank Balance: "<<thisPlayer.bank<<"\n";
		cout<<"Weapons:\n";
		cout<<con::fg_blue<<"1)"<<con::fg_white<<" Knife (cost: "<<((thisPlayer.knife + 1) * 100)<<")\n";
		cout<<con::fg_blue<<"2)"<<con::fg_white<<" Sword (cost: "<<((thisPlayer.sword + 1) * 500)<<")\n";
		cout<<con::fg_blue<<"3)"<<con::fg_white<<" Spear (cost: "<<((thisPlayer.spear + 1) * 600)<<")\n";
		cout<<con::fg_red<<"Armour:\n";
		cout<<con::fg_blue<<"4)"<<con::fg_white<<" Shield (cost: "<<((thisPlayer.shield + 1) * 200)<<")\n";
		cout<<con::fg_blue<<"5)"<<con::fg_white<<" Chestplate (cost: "<<((thisPlayer.chestplate + 1) * 400)<<")\n";
		cout<<con::fg_blue<<"6)"<<con::fg_white<<" Helm (cost: "<<((thisPlayer.helm + 1) * 600)<<")\n";
		cout<<con::fg_blue<<"\nother)"<<con::fg_white<<" Return to equips.\n";
		cout<<"Choice: ";
		cin>>inputnum;
	if (cin.fail()) { cin.clear(); inputnum = 100; }
		switch (inputnum)
		{
		default: sleep(1000); return 0; break;
			case 1: sleep(1000); {	if (thisPlayer.bank >= ((thisPlayer.knife + 1) * 100)) {
									thisPlayer.bank -= ((thisPlayer.knife + 1) * 100);
									thisPlayer.knife += 1;
								} else {
									cout<<"\n\nInsufficient Funds!\n";
								};}; sleep(1000); break;
			case 2: sleep(1000); {	if (thisPlayer.bank >= ((thisPlayer.sword + 1) * 500)) {
									thisPlayer.bank -= ((thisPlayer.sword + 1) * 500);
									thisPlayer.sword += 1;
								} else {
									cout<<"\n\nInsufficient Funds!\n";
								};}; sleep(1000); break;
			case 3: sleep(1000); {	if (thisPlayer.bank >= ((thisPlayer.spear + 1) * 600)) {
									thisPlayer.bank -= ((thisPlayer.spear + 1) * 600);
									thisPlayer.spear += 1;
								} else {
									cout<<"\n\nInsufficient Funds!\n";
								};}; sleep(1000); break;
			case 4: sleep(1000); {	if (thisPlayer.bank >= ((thisPlayer.shield + 1) * 200)) {
									thisPlayer.bank -= ((thisPlayer.shield + 1) * 200);
									thisPlayer.shield += 1;
								} else {
									cout<<"\n\nInsufficient Funds!\n";
								};}; sleep(1000); break;
			case 5: sleep(1000); {	if (thisPlayer.bank >= ((thisPlayer.chestplate + 1) * 400)) {
									thisPlayer.bank -= ((thisPlayer.chestplate + 1) * 400);
									thisPlayer.chestplate += 1;
								} else {
									cout<<"\n\nInsufficient Funds!\n";
								};}; sleep(1000); break;			
			case 6: sleep(1000); {	if (thisPlayer.bank >= ((thisPlayer.helm + 1) * 600)) {
									thisPlayer.bank -= ((thisPlayer.helm + 1) * 600);
									thisPlayer.helm += 1;
								} else {
									cout<<"\n\nInsufficient Funds!\n";
								};}; sleep(1000); break;
		};//case
	};//while
}
//--------------------------------------------------------------------------------------------------------------
//Information Functions ----------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------------------
int equips(player &thisPlayer,
		 item &wep1, item &wep2, item &wep3,
		 item &arm1, item &arm2, item &arm3)
{
	while (1 == 1) {
		con::clr( cout );
 cout<<con::fg_red<<"Player Equips:\n\n";
 cout<<"Weapons:\n";
 cout<<"Total Additional Attack: "<<thisPlayer.addAtk<<"\n";
 if (thisPlayer.knife == 0) {
	 cout<<con::fg_white<<"You have no knives, and no knife skills.\n";
 } else {
	 cout<<con::fg_white<<"You have worked at knives and gain "<<(thisPlayer.knife * wep1.atk)<<" additional damage\n";
 };
  if (thisPlayer.sword == 0) {
	 cout<<con::fg_white<<"You have no swords, and no skill with swords.\n";
 } else {
	 cout<<con::fg_white<<"You have worked with sword and gain "<<(thisPlayer.sword * wep2.atk)<<" additional damage\n";
 };
 if (thisPlayer.spear == 0) {
	 cout<<con::fg_white<<"You have no spears, and no skill with spears.\n\n";
 } else {
	 cout<<con::fg_white<<"You have worked with spears and gain "<<(thisPlayer.spear * wep3.atk)<<" additional damage\n\n";
 };

  cout<<con::fg_red<<"Armour:\n";
 cout<<"Total Additional Defence: "<<thisPlayer.addDef<<"\n";
 if (thisPlayer.shield == 0) {
	 cout<<con::fg_white<<"You have no shields.\n";
 } else {
	 cout<<con::fg_white<<"Have a shield and gain "<<(thisPlayer.shield * arm1.def)<<" additional defence\n";
 };
  if (thisPlayer.chestplate == 0) {
	 cout<<con::fg_white<<"You have no chestplate.\n";
 } else {
	 cout<<con::fg_white<<"You have a chestplate and gain "<<(thisPlayer.chestplate * arm2.def)<<" additional defence\n";
 };
 if (thisPlayer.helm == 0) {
	 cout<<con::fg_white<<"You have no helm.\n\n";
 } else {
	 cout<<con::fg_white<<"You have a helm and gain "<<(thisPlayer.helm * arm3.def)<<" additional defence\n\n";
 };
 cout<<con::fg_blue<<"\nother)"<<con::fg_white<<" Return to main menu.\n";
 cout<<con::fg_blue<<"1)"<<con::fg_white<<" Buy Equips.\n";
 cout<<"choice: ";
 cin>>inputnum;
	if (cin.fail()) { cin.clear(); inputnum = 100; }
 switch (inputnum) {
	 case 1: sleep(1000); buy(thisPlayer); break;
 default: sleep(1000); return 0; break;
 }
}
 return 0;
}

//--------------------------------------------------------------------------------------------------------------

int playerInfo(player &thisPlayer, monster &thisMonster)
{
		con::clr( cout );
    cout<<con::fg_red<<"\n\nPlayer Info:\n";
    cout<<"------------\n";
    cout<<con::fg_white<<"Level: "<< thisPlayer.level <<"\n";
	cout<<"Spirit Power: "<< thisPlayer.spirit <<"\n";
    cout<<"Exp: "<< thisPlayer.experience <<"\n";
    cout<<"Health "<< thisPlayer.health <<"/"<< thisPlayer.maxhealth <<"\n";
    cout<<"atk: "<< thisPlayer.atk <<"+"<<thisPlayer.addAtk<<" Def:"<< thisPlayer.def <<"+"<<thisPlayer.addDef<<" Stat Points:"<<thisPlayer.stat<<"\n";
    cout<<"Bank Balance: "<<thisPlayer.bank<<"\n\n";
    cout<<con::fg_red<<"Monster info:\n";
	cout<<"-------------\n";
    cout<<con::fg_white<<"level: "<<thisMonster.level<<" atk/def: "<< thisMonster.atk<<"/"<<thisMonster.def<<"\n";
    cout<<"current hp: "<<thisMonster.hp<<" total hp: "<<thisMonster.maxhp<<"\n";
	sleep(1000);
	cout<<con::fg_blue<<"anything)"<<con::fg_white<<" Return to main menu\nChoice:";
	cin>>inputnum;
	if (cin.fail()) { cin.clear(); inputnum = 100; }
	    switch (inputnum)
    {
	default: return 0; break;
	case 1: return 0; break;
		};

}

//--------------------------------------------------------------------------------------------------------------
//drawing functions --------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------------------

int give()
{
		con::clr( cout );
 cout<<con::fg_red<<"  ( )   "<<con::fg_blue<<"( )\n"<<
	   con::fg_red<<"  ())   "<<con::fg_blue<<"(()\n"<<
	   con::fg_red<<"  ()|   "<<con::fg_blue<<"|()\n"<<
	   con::fg_red<<"   ()   "<<con::fg_blue<<"()\n"<<
	   con::fg_red<<"   ()   "<<con::fg_blue<<"()\n"<<
	   con::fg_red<<"   ''   "<<con::fg_blue<<"''\n";
 sleep(100);
		con::clr( cout );
 cout<<con::fg_red<<"  ( )   "<<con::fg_blue<<"( )\n"<<
	   con::fg_red<<"  ())   "<<con::fg_blue<<"(()\n"<<
	   con::fg_red<<"  (()   "<<con::fg_blue<<"|()\n"<<
	   con::fg_red<<"   ()   "<<con::fg_blue<<"()\n"<<
	   con::fg_red<<"   ()   "<<con::fg_blue<<"()\n"<<
	   con::fg_red<<"   ''   "<<con::fg_blue<<"''\n";
sleep(100);
		con::clr( cout );
 cout<<con::fg_red<<"  ( )   "<<con::fg_blue<<"( )\n"<<
	   con::fg_red<<"  ())   "<<con::fg_blue<<"(()\n"<<
 	   con::fg_red<<"  ( ()  "<<con::fg_blue<<"|()\n"<<
  	   con::fg_red<<"   ()   "<<con::fg_blue<<"()\n"<<
  	   con::fg_red<<"   ()   "<<con::fg_blue<<"()\n"<<
  	   con::fg_red<<"   ''   "<<con::fg_blue<<"''\n";
sleep(100);
		con::clr( cout );
 cout<<con::fg_red<<"  ( )   "<<con::fg_blue<<"( )\n"<<
 	   con::fg_red<<"  (()   "<<con::fg_blue<<"(()\n"<<
 	   con::fg_red<<"  (  () "<<con::fg_blue<<"|()\n"<<
 	   con::fg_red<<"   ()   "<<con::fg_blue<<"()\n"<<
 	   con::fg_red<<"   ()   "<<con::fg_blue<<"()\n"<<
 	   con::fg_red<<"   ''   "<<con::fg_blue<<"''\n";
 sleep(100);
		con::clr( cout );
 cout<<con::fg_red<<"  ( )   "<<con::fg_blue<<"( )\n"<<
 	   con::fg_red<<"  (( ) )"<<con::fg_blue<<"(()\n"<<
 	   con::fg_red<<"  ( )   "<<con::fg_blue<<"|()\n"<<
 	   con::fg_red<<"   ()   "<<con::fg_blue<<"()\n"<<
 	   con::fg_red<<"   ()   "<<con::fg_blue<<"()\n"<<
 	   con::fg_red<<"   ''   "<<con::fg_blue<<"''\n";
sleep(100);
		con::clr( cout );
 cout<<con::fg_red<<"  ( )    "<<con::fg_blue<<"( )\n"<<
 	   con::fg_red<<"  (( ) ) "<<con::fg_blue<<"(()\n"<<
 	   con::fg_red<<"  ( )    "<<con::fg_blue<<"| ()\n"<<
 	   con::fg_red<<"   ()    "<<con::fg_blue<<"()\n"<<
 	   con::fg_red<<"   ()   "<<con::fg_blue<<"()\n"<<
  	   con::fg_red<<"   ''   "<<con::fg_blue<<"''\n";
 sleep(100);
 		con::clr( cout );
 return 0;
}

//--------------------------------------------------------------------------------------------------------------

int take()
{
		con::clr( cout );
 cout<<con::fg_red<<"  ( )"<<con::fg_blue<<"   ( )\n"<<
					 con::fg_red<<"  ())"<<con::fg_blue<<"   (()\n"<<
					 con::fg_red<<"  ()|"<<con::fg_blue<<"   |()\n"<<
					 con::fg_red<<"   ()"<<con::fg_blue<<"   ()\n"<<
					 con::fg_red<<"   ()"<<con::fg_blue<<"   ()\n"<<
					 con::fg_red<<"   ''"<<con::fg_blue<<"   ''\n";
 sleep(100);
		con::clr( cout );
 cout<<con::fg_red<<"  ( )"<<con::fg_blue<<"   ( )\n"<<
					 con::fg_red<<"  ())"<<con::fg_blue<<"   (()\n"<<
					 con::fg_red<<"  ()|"<<con::fg_blue<<"   ())\n"<<
					 con::fg_red<<"   ()"<<con::fg_blue<<"   ()\n"<<
					 con::fg_red<<"   ()"<<con::fg_blue<<"   ()\n"<<
					 con::fg_red<<"   ''"<<con::fg_blue<<"   ''\n";
sleep(100);

		con::clr( cout );
 cout<<con::fg_red<<"  ( )"<<con::fg_blue<<"   ( )\n"<<
					 con::fg_red<<"  ())"<<con::fg_blue<<"   (()\n"<<
 					 con::fg_red<<"  ()|"<<con::fg_blue<<"  () )\n"<<
  					 con::fg_red<<"   ()"<<con::fg_blue<<"   ()\n"<<
  					 con::fg_red<<"   ()"<<con::fg_blue<<"   ()\n"<<
  					 con::fg_red<<"   ''"<<con::fg_blue<<"   ''\n";
sleep(100);
		con::clr( cout );
 cout<<con::fg_red<<"  ( )"<<con::fg_blue<<"   ( )\n"<<
 					 con::fg_red<<"  (()"<<con::fg_blue<<"   (()\n"<<
 					 con::fg_red<<"  ()|"<<con::fg_blue<<" ()  )\n"<<
 					 con::fg_red<<"   ()"<<con::fg_blue<<"   ()\n"<<
 					 con::fg_red<<"   ()"<<con::fg_blue<<"   ()\n"<<
 					 con::fg_red<<"   ''"<<con::fg_blue<<"   ''\n";
 sleep(100);
		con::clr( cout );
 cout<<con::fg_red<<"  ( )"<<con::fg_blue<<"   ( )\n"<<
 					 con::fg_red<<"  ())"<<con::fg_blue<<"( ( ))\n"<<
 					 con::fg_red<<"  ())"<<con::fg_blue<<"   |()\n"<<
 					 con::fg_red<<"   ()"<<con::fg_blue<<"   ()\n"<<
 					 con::fg_red<<"   ()"<<con::fg_blue<<"   ()\n"<<
 					 con::fg_red<<"   ''"<<con::fg_blue<<"   ''\n";
sleep(100);
		con::clr( cout );
 cout<<con::fg_red<<" ( )"<<con::fg_blue<<"    ( )\n"<<
 					 con::fg_red<<" ())"<<con::fg_blue<<" ( ( ))\n"<<
 					 con::fg_red<<"() |"<<con::fg_blue<<"    ( )\n"<<
 					 con::fg_red<<"  ()"<<con::fg_blue<<"     ()\n"<<
 					 con::fg_red<<"   ()"<<con::fg_blue<<"    ()\n"<<
  					 con::fg_red<<"   ''"<<con::fg_blue<<"    ''\n";
 sleep(100);
 		con::clr( cout );
return 0;
}

//--------------------------------------------------------------------------------------------------------------

int drawMenu(player &thisPlayer)
{

if (thisPlayer.health < (thisPlayer.maxhealth / 4))
{
cout<<" ~~~'\n"<<
	  "|> <|\n"<<
	  "{ _ }\n"<<
	  " '-'\n";
} else if (thisPlayer.health < (thisPlayer.maxhealth / 2)) 
{
cout<<" ~~~'\n"<<
	  "|o o|\n"<<
	  "{ _ }\n"<<
	  " '-'\n";
} else if (thisPlayer.health < ((thisPlayer.maxhealth / 4) * 3))
{
cout<<" ~~~'\n"<<
	  "|^ ^|\n"<<
	  "{ _ }\n"<<
	  " '-'\n";

} else {
cout<<" ~~~'\n"<<
	  "|^ ^|\n"<<
	  "{)_(}\n"<<
 	  " '-'\n";
};
return 0;
}

//--------------------------------------------------------------------------------------------------------------

int drawSpirit()
{
		con::clr( cout );
cout<<con::fg_red<<
"  |( )  |\n"<<
"| (( ))\n"<<
" ()( )()\n"<<
" |() ()\n"<<
"  () () |\n";
sleep(250);
		con::clr( cout );
cout<<
"|  ( )   \n"<<
"  (( ))\n"<<
" |)( )()\n"<<
"  () () |\n"<<
"  |) ()  \n";
sleep(250);
		con::clr( cout );
cout<<
"   ( )   \n"<<
" |(( ))\n"<<
" ()( )()|\n"<<
"  |) ()  \n"<<
"| () () |\n";
sleep(500);
		con::clr( cout );
cout<<
"   ( )   \n"<<
"  (( ))\n"<<
"() ( ) () \n"<<
"  () ()  \n"<<
"  () ()  \n";
cout<<"You feel stronger!\n";
sleep(1000);
		con::clr( cout );

return 0;

}

//--------------------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------------------