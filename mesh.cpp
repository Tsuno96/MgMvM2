#include "mesh.h"
#include <GL/glut.h>
#include <stdio.h>
#include <string.h>
#include <math.h>
#include <stdlib.h>



Point3f operator+ (const Point3f & p1, const Point3f & p2) {
    return Point3f (p1.x + p2.x, p1.y + p2.y, p1.z + p2.z);
}

Point3f operator- (const Point3f & p1, const Point3f & p2) {
    return Point3f (p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);
}

Point3f operator- (const Point3f & p) {
    return Point3f (-p.x, -p.y, -p.z);
}

Point3f operator/ (const Point3f & p, float divisor) {
    return Point3f (p.x/divisor, p.y/divisor, p.z/divisor);
}



Mesh::Mesh(){}
using namespace std;

/************************************************************
 * Fonction de calcul du cube englobant le maillage
 ************************************************************/
void Mesh::computeBoundingCube () {

    Point3f bbCenter = Point3f(0.,0.,0.);
    for  (unsigned int i = 0; i < vertices.size (); i++)
        bbCenter += vertices[i];
    bbCenter /= vertices.size ();
    float bbHalfEdge = (vertices[0] - bbCenter).norm();
    for (unsigned int i = 0; i < vertices.size (); i++){
        float m = (vertices[i] - bbCenter).norm();
        if (m > bbHalfEdge)
            bbHalfEdge = m;
    }

    //Elargissement
    bbHalfEdge += 0.01;

    origin = Point3f(bbCenter.x - bbHalfEdge, bbCenter.y - bbHalfEdge, bbCenter.z - bbHalfEdge);
    cubeSize = bbHalfEdge*2.;

}



/************************************************************
 * Fonctions de calcul des normales pour chaque sommet
 ************************************************************/
void Mesh::computeVertexNormals () {
    normals.clear();

    //initialisation des normales des vertex
    normals.resize(vertices.size(), Point3f (0.0, 0.0, 0.0));

    //Somme des normales du 1 voisinage du vertex
    for (unsigned int i = 0; i < triangles.size (); i++) {
        Point3f edge01 = vertices[triangles[i].index[1]] -  vertices[triangles[i].index[0]];
        Point3f edge02 = vertices[triangles[i].index[2]] -  vertices[triangles[i].index[0]];
        Point3f n = Point3f::crossProduct (edge01, edge02);
        n.normalize ();
        for (unsigned int j = 0; j < 3; j++)
            normals[triangles[i].index[j]] += n;
    }

    //Normalisation
    for (unsigned int i = 0; i < vertices.size (); i++)
        normals[i].normalize ();
}

/************************************************************
 * Recentre et ajuste la taille des sommets du maillage
 ************************************************************/
void Mesh::centerAndScaleToUnit () {
    Point3f c;
    for  (unsigned int i = 0; i < vertices.size (); i++)
        c += vertices[i];
    c /= vertices.size ();
    float maxD = (vertices[0] - c).norm();
    for (unsigned int i = 0; i < vertices.size (); i++){
        float m = (vertices[i] - c).norm();
        if (m > maxD)
            maxD = m;
    }
    for  (unsigned int i = 0; i < vertices.size (); i++)
        vertices[i] = (vertices[i] - c) / maxD;
}


/************************************************************
 * Fonctions de dessin
 ************************************************************/
void Mesh::drawSmooth(){

    glBegin(GL_TRIANGLES);

    for (int i=0;i<triangles.size();++i)
    {
        for(int v = 0; v < 3 ; v++){
            glNormal3f(normals[triangles[i].index[v]].x, normals[triangles[i].index[v]].y, normals[triangles[i].index[v]].z);
            glVertex3f(vertices[triangles[i].index[v]].x, vertices[triangles[i].index[v]].y , vertices[triangles[i].index[v]].z);
        }

    }
    glEnd();
}

void Mesh::draw(){
    glBegin(GL_TRIANGLES);

    for (int i=0;i<triangles.size();++i)
    {
        Point3f edge01 = vertices[triangles[i].index[1]] -  vertices[triangles[i].index[0]];
        Point3f edge02 = vertices[triangles[i].index[2]] -  vertices[triangles[i].index[0]];
        Point3f n = Point3f::crossProduct (edge01, edge02);
        n.normalize ();
        glNormal3f(n.x, n.y, n.z);
        for(int v = 0; v < 3 ; v++){
            glVertex3fv(vertices[triangles[i].index[v]].pos);
        }

    }
    glEnd();
}


/************************************************************
 * Fonctions de chargement du maillage
 ************************************************************/
bool Mesh::loadMesh(const char * filename)
{

    std::vector<int> vhandles;

    const unsigned int LINE_LEN=256;
    char s[LINE_LEN];
    FILE * in;
#ifdef WIN32
    errno_t error=fopen_s(&in, filename,"r");
    if (error!=0)
#else
        in = fopen(filename,"r");
    if (!(in))
#endif
        return false;

    float x, y, z;

    while(in && !feof(in) && fgets(s, LINE_LEN, in))
    {
        // material file
        // vertex
        if (strncmp(s, "v ", 2) == 0)
        {
            if (sscanf(s, "v %f %f %f", &x, &y, &z))
                vertices.push_back(Point3f(x,y,z));
        }
        // face
        else if (strncmp(s, "f ", 2) == 0)
        {
            int component(0), nV(0);
            bool endOfVertex(false);
            char *p0, *p1(s+2); //place behind the "f "

            vhandles.clear();

            while (*p1 == ' ') ++p1; // skip white-spaces

            while (p1)
            {
                p0 = p1;

                // overwrite next separator

                // skip '/', '\n', ' ', '\0', '\r' <-- don't forget Windows
                while (*p1 != '/' && *p1 != '\r' && *p1 != '\n' &&
                       *p1 != ' ' && *p1 != '\0')
                    ++p1;

                // detect end of vertex
                if (*p1 != '/') endOfVertex = true;

                // replace separator by '\0'
                if (*p1 != '\0')
                {
                    *p1 = '\0';
                    p1++; // point to next token
                }

                // detect end of line and break
                if (*p1 == '\0' || *p1 == '\n')
                    p1 = 0;


                // read next vertex component
                if (*p0 != '\0')
                {
                    switch (component)
                    {
                    case 0: // vertex
                        vhandles.push_back(atoi(p0)-1);
                        break;

                    case 1: // texture coord
                        //assert(!vhandles.empty());
                        //assert((unsigned int)(atoi(p0)-1) < texcoords.size());
                        //_bi.set_texcoord(vhandles.back(), texcoords[atoi(p0)-1]);
                        break;

                    case 2: // normal
                        //assert(!vhandles.empty());
                        //assert((unsigned int)(atoi(p0)-1) < normals.size());
                        //_bi.set_normal(vhandles.back(), normals[atoi(p0)-1]);
                        break;
                    }
                }

                ++component;

                if (endOfVertex)
                {
                    component = 0;
                    nV++;
                    endOfVertex = false;
                }
            }


            if (vhandles.size()>3)
            {
                //model is not triangulated, so let us do this on the fly...
                //to have a more uniform mesh, we add randomization
                unsigned int k=(false)?(rand()%vhandles.size()):0;
                for (unsigned int i=0;i<vhandles.size()-2;++i)
                {
                    triangles.push_back(Triangle(vhandles[(k+0)%vhandles.size()],vhandles[(k+i+1)%vhandles.size()],vhandles[(k+i+2)%vhandles.size()]));
                }
            }
            else if (vhandles.size()==3)
            {
                triangles.push_back(Triangle(vhandles[0],vhandles[1],vhandles[2]));
            }
            else
            {
                printf("TriMesh::LOAD: Unexpected number of face vertices (<3). Ignoring face");
            }
        }
        memset(&s, 0, LINE_LEN);
    }
    fclose(in);

    centerAndScaleToUnit ();
    computeVertexNormals();
    return true;
}
