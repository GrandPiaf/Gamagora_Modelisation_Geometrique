#ifdef WIN32
#include <windows.h>
#endif
#include <GL/glut.h>
#include <stdlib.h>
#include <math.h>
#include <assert.h>
#include "traqueboule.h"
#include "mesh.h"
#include "map"

using namespace std;

//modele original
Mesh mesh;
//votre modele simplifie
Mesh simplified;


//Options d'affichage
bool displayGrid = false;
bool simplifiedDisplay = false;
bool displayRepresentativeVertices=false;

typedef enum {Wireframe, Flat, Gouraud} PolygonMode;
//type d'affichage de polygons (a changer avec "w")
static PolygonMode polygonMode = Gouraud;



//resolution de la grille
unsigned int r = 16;

unsigned int W_fen = 800;  // largeur fenetre
unsigned int H_fen = 800;  // hauteur fenetre

// espace couvert par la fonction
const float xmin = -1.0, xmax=1.0, ymin=-1.0, ymax = 1.0;

//Origine du cube englobant
Point3f origin;
//Taille du cube englobant le maillage
float size;
                
//Liste des indices de sommets contenus dans chacune des cellules
vector<vector<unsigned int> > verticesInCell;
//Liste des representants des cellules
vector<Point3f> representants;


/************************************************************
 * Fonction retournant l'identifiant de la cellule dans
 * laquelle se trouve le sommet
 ************************************************************/
unsigned int getCellIDContaining(const Point3f & pos){
	//A REMPLIR
	Point3f diff = pos - origin;
    float step = r/size;

    unsigned int i = static_cast<unsigned int>(diff.x*step);
    unsigned int j = static_cast<unsigned int>(diff.y*step);
    unsigned int k = static_cast<unsigned int>(diff.z*step);

    return i + j*r + k*r*r;

}



/************************************************************
 * 
 *	A REMPLIR !!!!!!
 * 
 ************************************************************/




/************************************************************
 * Fonction plongeant les sommets dans la grille et
 * calculant le representant de chaque cellule
 ************************************************************/
void putVertices(const std::vector<Point3f> & vertices){

	//remplir les tableaux verticesInCell, representants
}


/************************************************************
 * Fonction de simplification du maillage
 ************************************************************/
//Astuce: utilisez un tableau (vector) d'entiers de la taille mesh.vertices.size 
//pour créer des nouveaux indices pour le maillage simplifié

void simplifyMesh(unsigned int _r){

    r = _r;

    const vector<Point3f> & vertices = mesh.vertices;
    const vector<Triangle> & triangles = mesh.triangles;

    putVertices(vertices);

    

	//A REMPLIR : remplissez les tableaux 
	vector<Point3f> simplifiedVertices;
    vector<Triangle> simplifiedTriangles;



	//creer le modele 
    simplified = Mesh(simplifiedVertices , simplifiedTriangles);
    simplified.computeVertexNormals();
}



/************************************************************
 * 
 *	FINI !!!!!! Regardez la fonction "dessiner" aussi. 
 *	Ajoutez des fonctions de debuggage si necessaire.
 * 
 ************************************************************/






/************************************************************
 * Fonction de dessin d'une cellule de la grille
 ************************************************************/
void drawCell(const Point3f & min,const Point3f& Max) {

    const Point3f corners[8] =
    {
        Point3f(min.x,min.y,min.z),
        Point3f(Max.x,min.y,min.z),
        Point3f(min.x,Max.y,min.z),
        Point3f(Max.x,Max.y,min.z),
        Point3f(min.x,min.y,Max.z),
        Point3f(Max.x,min.y,Max.z),
        Point3f(min.x,Max.y,Max.z),
        Point3f(Max.x,Max.y,Max.z)
    };


    static const unsigned short faceCorners[6][4] =
    {
        { 0,4,6,2 },
        { 5,1,3,7 },
        { 0,1,5,4 },
        { 3,2,6,7 },
        { 0,2,3,1 },
        { 6,4,5,7 }
    };

    glBegin(GL_QUADS);
    for (unsigned short f=0;f<6;++f)
    {
        const unsigned short* face = faceCorners[f];
        for(unsigned int v = 0; v<4; v++)
            glVertex3f(corners[face[v]].x, corners[face[v]].y, corners[face[v]].z);

    }
    glEnd();

}

/************************************************************
 * Fonction de dessin de la grille
 ************************************************************/
void drawGrid(){

	//changez la fonction pour afficher seulement les cellules avec du contenu

	glPushAttrib(GL_ALL_ATTRIB_BITS);
    glLineWidth(1.0f);
    glColor3f(1.0f,1.0f,0.0f);
    glDisable(GL_LIGHTING);
    glPolygonMode(GL_FRONT_AND_BACK,GL_LINE);

    float step = size / r;
    for (unsigned int i=0;i<r;i++)
    {
        for (unsigned int j=0;j<r;j++)
        {
            for (unsigned int k=0;k<r;k++)
            {
                unsigned int cellId = i + j*r + k*r*r;
                //ne dessinez pas la cellule, si la cellule est vide.
				drawCell(origin+Point3f(i*step,j*step,k*step), origin+Point3f((i+1)*step,(j+1)*step,(k+1)*step));
            }
        }
    }
    glPopAttrib();
}

/************************************************************
 * Fonction pour initialiser le maillage
 ************************************************************/
void init(const char * fileName){

    mesh.loadMesh(fileName);

    mesh.computeBoundingCube ();

    //Initialisation du cube englobant
    origin = mesh.origin;
    size = mesh.cubeSize;

    //Initialisation de la grille
    r = 16;

    //Initialisation du maillage simplifié
    simplified = mesh;
    
    simplifyMesh(r);
}



/************************************************************
 * Appel des différentes fonctions de dessin
************************************************************/
void dessinerRepere(float length)
{
    glDisable(GL_LIGHTING);

    glBegin(GL_LINES);
    glColor3f(1,0,0);
    glVertex3f(0,0,0);
    glVertex3f(length,0,0);

    glColor3f(0,1,0);
    glVertex3f(0,0,0);
    glVertex3f(0,length,0);

    glColor3f(0,0,1);
    glVertex3f(0,0,0);
    glVertex3f(0,0,length);
    glEnd();
    glEnable(GL_LIGHTING);

}

void displayRepresentatives()
{
    glDisable(GL_LIGHTING);
    glPointSize(5.);
    glBegin(GL_POINTS);


    for(unsigned int i = 0; i< representants.size(); i++){
        glVertex3fv(representants[i].pos);
    }
    glEnd();
    glEnable(GL_LIGHTING);
}


void dessiner( )
{
	if (displayRepresentativeVertices)
	{
		displayRepresentatives();
	}


    if (polygonMode != Gouraud)
        simplifiedDisplay? simplified.draw() : mesh.draw();
    else
        simplifiedDisplay? simplified.drawSmooth() : mesh.drawSmooth();

    if (displayGrid)
        drawGrid();

    return;

}

void animate(){}
void display(void);
void reshape(int w, int h);
void keyboard(unsigned char key, int x, int y);

void printUsage () {
}

/************************************************************
 * Programme principal
 ************************************************************/
int main(int argc, char** argv)
{

    if (argc > 2) {
        printUsage ();
        return -1;
    }
    glutInit (&argc, argv);

    init(argc == 2 ? argv[1] : "./models/bunny.obj");

    // couches du framebuffer utilisees par l'application
    glutInitDisplayMode( GLUT_DOUBLE | GLUT_RGBA | GLUT_DEPTH );

    // position et taille de la fenetre
    glutInitWindowPosition(200, 100);
    glutInitWindowSize(W_fen,H_fen);
    glutCreateWindow(argv[0]);	

    // Initialisation du point de vue
    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();
    glTranslatef(0,0,-4);
    tbInitTransform();     // initialisation du point de vue
    tbHelp();                      // affiche l'aide sur la traqueboule

    //
    // Active la lumière
    // Pour la partie
    // ECLAIRAGE

    glEnable( GL_LIGHTING );
    glEnable( GL_LIGHT0 );
    glEnable(GL_COLOR_MATERIAL);
    int LightPos[4] = {0,0,3,1};
    int MatSpec [4] = {1,1,1,1};
    glLightiv(GL_LIGHT0,GL_POSITION,LightPos);
    glMaterialiv(GL_FRONT_AND_BACK,GL_SPECULAR,MatSpec);
    glMateriali(GL_FRONT_AND_BACK,GL_SHININESS,10);
    glEnable(GL_NORMALIZE);
    

    // cablage des callback
    glutReshapeFunc(reshape);
    glutKeyboardFunc(keyboard);
    glutDisplayFunc(display);
    glutMouseFunc(tbMouseFunc);    // traqueboule utilise la souris
    glutMotionFunc(tbMotionFunc);  // traqueboule utilise la souris
    glutIdleFunc( animate);

    // lancement de la boucle principale
    glutMainLoop();

    return 0;  // instruction jamais exécutée
}


/************************************************************
 * Fonctions de gestion opengl à ne pas toucher
 ************************************************************/
// Actions d'affichage
// Ne pas changer
void display(void)
{
    // Details sur le mode de tracé
    glEnable( GL_DEPTH_TEST );            // effectuer le test de profondeur

    if(polygonMode == Wireframe){
        glPolygonMode (GL_FRONT_AND_BACK, GL_LINE);
    } else {
        glPolygonMode (GL_FRONT_AND_BACK, GL_FILL);
    }

    glShadeModel(GL_SMOOTH);

    // Effacer tout
    glClearColor (0.0, 0.0, 0.0, 0.0);
    glClear( GL_COLOR_BUFFER_BIT  | GL_DEPTH_BUFFER_BIT); // la couleur et le z
    

    glLoadIdentity();  // repere camera

    tbVisuTransform(); // origine et orientation de la scene

    dessiner( );    

    glutSwapBuffers();
}

// pour changement de taille ou desiconification
void reshape(int w, int h)
{
    glViewport(0, 0, (GLsizei) w, (GLsizei) h);
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    gluPerspective (50, (float)w/h, 1, 10);
    glMatrixMode(GL_MODELVIEW);
}

// prise en compte du clavier
void keyboard(unsigned char key, int x, int y)
{
    printf("key %d pressed at %d,%d\n",key,x,y);
    fflush(stdout);
    switch (key)
    {
    case 'w':
        if (polygonMode == Flat) {
            polygonMode = Wireframe;
        } else if (polygonMode == Wireframe) {
            polygonMode = Gouraud;
        } else {
            polygonMode = Flat;
        }
        break;
    case 'g':
        displayGrid = !displayGrid;
        break;
    case 's':
        simplifiedDisplay = ! simplifiedDisplay;
        break;
    case 'r':
		displayRepresentativeVertices= ! displayRepresentativeVertices;
        break;
    case '1':
        simplifyMesh(64);
        break;
    case '2':
        simplifyMesh(32);
        break;
    case '3':
        simplifyMesh(16);
        break;
    case 27:     // touche ESC
        exit(0);
    }
    display();
}

