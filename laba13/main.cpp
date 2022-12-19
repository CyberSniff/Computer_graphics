#define _CRT_SECURE_NO_DEPRECATE

#include <gl/glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <iostream>
#include <vector>
#include <string>

using namespace std;

unsigned int WIDTH = 1920;
unsigned int HEIGHT = 1080;

GLuint Program;
GLuint Attrib_vertex, Attrib_texture;
GLuint Unif_model, Unif_view, Unif_projection;

GLuint VAO_sun, VBO_pos_coord_sun, VBO_text_coord_sun, texture_sun;
sf::Image texture_image_sun;
unsigned int vertices_counter_sun;
glm::mat4 scale_sun, rotate_sun, translate_sun;

GLuint VAO_planets, VBO_pos_coord_planets, VBO_text_coord_planets, texture_planets;
sf::Image texture_image_planets;
unsigned int vertices_counter_planets;
glm::mat4 scale_planets, rotate_planets, translate_planets;
vector<glm::vec3>translations_planet;
glm::mat4 rotate_planets_step;

glm::mat4 model, view, projection;

glm::vec3 camera_pos, camera_front, camera_up;
GLfloat yaw, pitch, last_x, last_y;
GLfloat sensitivity, camera_speed;

const char* VertexShaderSource = R"(
    #version 330 core
    in vec2 text_coord;
    in vec3 coord;

    out vec2 vert_text_coord;

    uniform mat4 model;
    uniform mat4 view;
    uniform mat4 projection;

    void main() {
        gl_Position = projection * view * model * vec4(coord, 1.0);

        vert_text_coord = text_coord;
    }
)";

const char* FragmentShaderSource = R"(
    #version 330 core
    in vec2 vert_text_coord;

    uniform sampler2D frag_text;

    void main() {
        vec4 text = texture(frag_text, vert_text_coord);
        gl_FragColor =  text;
    }
)";

void Init();
void Draw();

int main() {
    sf::Window window(sf::VideoMode(WIDTH, HEIGHT), "Task 13", sf::Style::Close, sf::ContextSettings(24));
    window.setVerticalSyncEnabled(true);
    window.setActive(true);

    sf::Mouse::setPosition(sf::Vector2i(WIDTH / 2, HEIGHT / 2));

    glewInit();

    Init();

    while (window.isOpen()) 
    {
        sf::Event event;
        while (window.pollEvent(event)) 
        {
            if (event.type == sf::Event::Closed) 
            {
                window.close();
            }
            else if (event.type == sf::Event::Resized) 
            {
                glViewport(0, 0, event.size.width, event.size.height);
            }
            else if (event.type == sf::Event::KeyPressed) 
            {
                switch (event.key.code) 
                {
                case(sf::Keyboard::W):camera_pos += camera_speed * camera_front; break;
                case(sf::Keyboard::S):camera_pos -= camera_speed * camera_front; break;
                case(sf::Keyboard::A):camera_pos -= glm::normalize(glm::cross(camera_front, camera_up)) * camera_speed; break;
                case(sf::Keyboard::D):camera_pos += glm::normalize(glm::cross(camera_front, camera_up)) * camera_speed; break;
                case(sf::Keyboard::Up):camera_pos += camera_speed * camera_up; break;
                case(sf::Keyboard::Down):camera_pos -= camera_speed * camera_up; break;
                default: break;
                }
            }
            else if (event.type == sf::Event::MouseMoved) 
            {
                sf::Vector2i pos = sf::Mouse::getPosition(window);
                int x_pos = pos.x;
                int y_pos = pos.y;

                GLfloat xoffset = x_pos - last_x;
                GLfloat yoffset = last_y - y_pos; 
                last_x = x_pos;
                last_y = y_pos;

                xoffset *= sensitivity;
                yoffset *= sensitivity;

                yaw += xoffset;
                pitch += yoffset;

                if (pitch > 89.0f)
                    pitch = 89.0f;
                if (pitch < -89.0f)
                    pitch = -89.0f;

                glm::vec3 front;
                front.x = cos(glm::radians(yaw)) * cos(glm::radians(pitch));
                front.y = sin(glm::radians(pitch));
                front.z = sin(glm::radians(yaw)) * cos(glm::radians(pitch));
                camera_front = glm::normalize(front);
            }
        }

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        Draw();

        window.display();
    }

    glDeleteProgram(Program);
    //glDeleteBuffers(1, &VBO_position);
    //glDeleteBuffers(1, &VBO_text_coord);
    //glDeleteBuffers(1, &VAO);

    return 0;
}

void checkOpenGLerror() 
{
    GLenum errCode;
    // https://www.khronos.org/opengl/wiki/OpenGL_Error
    if ((errCode = glGetError()) != GL_NO_ERROR)
        cout << "OpenGl error!: " << errCode << endl;
}

void ShaderLog(unsigned int shader) 
{
    int infologLen = 0;
    int charsWritten = 0;
    char* infoLog;
    glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &infologLen);
    if (infologLen > 1)
    {
        infoLog = new char[infologLen];
        if (infoLog == NULL)
        {
            cout << "Error: Could not allocate InfoLog buffer" << endl;
            exit(1);
        }
        glGetShaderInfoLog(shader, infologLen, &charsWritten, infoLog);
        cout << "InfoLog: " << infoLog << "\n\n\n";
        delete[] infoLog;
    }
}

void LoadOBJ(string file_path, vector<glm::vec3>& pos_coord, vector<glm::vec2>& text_coord)
{
    FILE* file = fopen(file_path.c_str(), "r");
    if (file == NULL) {
        std::cout << "Error: Impossible to open the file" << file_path << " \n";
        return;
    }

    vector<glm::vec3> temp_pos_coord;
    vector<glm::vec2> temp_text_coord;
    vector<unsigned int>pos_coord_indices, text_coord_indices;

    while (1) {

        char line_header[128];
        int res = fscanf(file, "%s", line_header);
        if (res == EOF)
            break;

        else
        {
            if (strcmp(line_header, "v") == 0) {
                glm::vec3 pos_coord;
                fscanf(file, "%f %f %f\n", &pos_coord.x, &pos_coord.y, &pos_coord.z);
                temp_pos_coord.push_back(pos_coord);
            }
            else if (strcmp(line_header, "vt") == 0) {
                glm::vec2 text_coord;
                fscanf(file, "%f %f\n", &text_coord.x, &text_coord.y);
                temp_text_coord.push_back(text_coord);
            }
            else if (strcmp(line_header, "f") == 0) {
                unsigned int pos_coord_index[4], text_coord_index[4], normal_index[4];
                int matches = fscanf(file, "%d/%d/%d %d/%d/%d %d/%d/%d %d/%d/%d\n", &pos_coord_index[0], &text_coord_index[0], &normal_index[0], &pos_coord_index[1], &text_coord_index[1], &normal_index[1], &pos_coord_index[2], &text_coord_index[2], &normal_index[2], &pos_coord_index[3], &text_coord_index[3], &normal_index[3]);
                if (matches == 9) {
                    pos_coord_indices.push_back(pos_coord_index[0]);
                    pos_coord_indices.push_back(pos_coord_index[1]);
                    pos_coord_indices.push_back(pos_coord_index[2]);
                    text_coord_indices.push_back(text_coord_index[0]);
                    text_coord_indices.push_back(text_coord_index[1]);
                    text_coord_indices.push_back(text_coord_index[2]);
                }
                else if (matches == 12)
                {
                    pos_coord_indices.push_back(pos_coord_index[0]);
                    pos_coord_indices.push_back(pos_coord_index[1]);
                    pos_coord_indices.push_back(pos_coord_index[2]);
                    pos_coord_indices.push_back(pos_coord_index[3]);
                    text_coord_indices.push_back(text_coord_index[0]);
                    text_coord_indices.push_back(text_coord_index[1]);
                    text_coord_indices.push_back(text_coord_index[2]);
                    text_coord_indices.push_back(text_coord_index[3]);
                }
            }
        }
    }

    for (unsigned int i = 0; i < pos_coord_indices.size(); i++) {
        unsigned int pos_coord_index = pos_coord_indices[i];
        glm::vec3 vertex = temp_pos_coord[pos_coord_index - 1];
        pos_coord.push_back(vertex);
        glm::vec2 vertex_uv = temp_text_coord[pos_coord_index - 1];
        text_coord.push_back(vertex_uv);
    }
}

void InitVAO(string path, GLuint& VAO, GLuint& VBO_pos_coord, GLuint& VBO_text_coord, unsigned int& vertices_counter) 
{
    vector<glm::vec3> pos_coord; vector<glm::vec2> text_coord;

    LoadOBJ(path, pos_coord, text_coord);

    vertices_counter = pos_coord.size();

    glGenVertexArrays(1, &VAO);
    glGenBuffers(1, &VBO_pos_coord);
    glGenBuffers(1, &VBO_text_coord);

    glBindVertexArray(VAO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_text_coord);
    glBufferData(GL_ARRAY_BUFFER, text_coord.size() * sizeof(glm::vec2), &text_coord[0], GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_pos_coord);
    glBufferData(GL_ARRAY_BUFFER, pos_coord.size() * sizeof(glm::vec3), &pos_coord[0], GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_pos_coord);
    glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 0, 0);
    glEnableVertexAttribArray(Attrib_vertex);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_text_coord);
    glVertexAttribPointer(Attrib_texture, 2, GL_FLOAT, GL_FALSE, 0, 0);
    glEnableVertexAttribArray(Attrib_texture);

    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindVertexArray(0);

    glDisableVertexAttribArray(Attrib_vertex);
    glDisableVertexAttribArray(Attrib_texture);

    checkOpenGLerror();
}


void InitShader() 
{
    GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vShader, 1, &VertexShaderSource, NULL);
    glCompileShader(vShader);
    //std::cout << "vertex shader \n";
    ShaderLog(vShader);

    GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fShader, 1, &FragmentShaderSource, NULL);
    glCompileShader(fShader);
    //std::cout << "fragment shader \n";
    ShaderLog(fShader);

    Program = glCreateProgram();
    glAttachShader(Program, vShader);
    glAttachShader(Program, fShader);
    glLinkProgram(Program);

    int link_ok;
    glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);
    if (!link_ok) {
        cout << "Error: Could not attach shaders \n";
        return;
    }

    Attrib_vertex = glGetAttribLocation(Program, "coord");
    if (Attrib_vertex == -1)
    {
        cout << "Error: Could not bind attrib coord" << endl;
        return;
    }

    Attrib_texture = glGetAttribLocation(Program, "text_coord");
    if (Attrib_texture == -1)
    {
        cout << "Error: Could not bind attrib texture" << endl;
        return;
    }

    Unif_model = glGetUniformLocation(Program, "model");
    if (Unif_model == -1)
    {
        cout << "Error: Could not bind unif model" << endl;
        return;
    }

    Unif_view = glGetUniformLocation(Program, "view");
    if (Unif_view == -1)
    {
        cout << "Error: Could not bind unif view" << endl;
        return;
    }

    Unif_projection = glGetUniformLocation(Program, "projection");
    if (Unif_projection == -1)
    {
        cout << "Error: Could not bind unif projection" << endl;
        return;
    }

    glDeleteShader(vShader);
    glDeleteShader(fShader);

    checkOpenGLerror();
}

void InitModel() 
{
    scale_sun = glm::scale(glm::mat4(1.0f), glm::vec3(0.3f, 0.3f, 0.3f));
    rotate_sun = glm::rotate(glm::mat4(1.0f), glm::radians(45.0f), glm::vec3(0.0f, 0.0f, 1.0f));
    translate_sun = glm::translate(glm::mat4(1.0f), glm::vec3(0.0f, 0.0f, -5.0f));

    scale_planets = glm::mat4(1.0f);
    rotate_planets = glm::rotate(glm::mat4(1.0f), glm::radians(-90.0f), glm::vec3(0.0f, 1.0f, 0.0f));
    translations_planet = { {-5.5f, 5.0f, -1.0f}, {8.0f, 5.5f, -3.0f}, {-4.0f, -4.0f, 0.0f}, 
                            {3.0f, 3.5f, 3.0f}, {6.0f, 0.0f, 4.0f}, {-6.5f, -3.5f, -2.0f}, 
                            {0.0f, 2.0f, 5.0f}, {2.5f, -2.5f, -2.5f}, {1.0f, 3.0f, -2.5f} };
    rotate_planets_step = glm::mat4(1.0f);
}

void InitProjection()
{
    projection = glm::perspective(45.0f, (GLfloat)WIDTH / (GLfloat)HEIGHT, 0.1f, 100.0f);
}

void InitTexture(string file_path, sf::Image& texture_image, GLuint& texture)
{
    if (!texture_image.loadFromFile(file_path))
    {
        std::cout << "Error: Could not load texture" << std::endl;
        return;
    }
    texture_image.flipVertically();
    glGenTextures(1, &texture);
    glBindTexture(GL_TEXTURE_2D, texture);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, texture_image.getSize().x, texture_image.getSize().y, 0, GL_RGBA, GL_UNSIGNED_BYTE, texture_image.getPixelsPtr());
    glGenerateMipmap(GL_TEXTURE_2D);

    glBindTexture(GL_TEXTURE_2D, 0);
}

void InitCamera()
{
    camera_pos = glm::vec3(0.0f, 0.0f, 10.0f);
    camera_front = glm::vec3(0.0f, 0.0f, -1.0f);
    camera_up = glm::vec3(0.0f, 1.0f, 0.0f);

    sensitivity = 0.05f;
    camera_speed = 0.05f;
    yaw = -90.0f;
    pitch = 0.0f;
    last_x = WIDTH / 2.0;
    last_y = HEIGHT / 2.0;
}

void Init() {
    InitShader();
    InitProjection();
    InitModel();
    InitCamera();

    InitVAO("OBJs/strawberry.obj", VAO_sun, VBO_pos_coord_sun, VBO_text_coord_sun, vertices_counter_sun);
    InitTexture("Textures/strawberry_texture.jpg", texture_image_sun, texture_sun);

    InitVAO("OBJs/television.obj", VAO_planets, VBO_pos_coord_planets, VBO_text_coord_planets, vertices_counter_planets);
    InitTexture("Textures/television_texture.jpg", texture_image_planets, texture_planets);

    glClearColor(0.8f, 0.4f, 1.0f, 1.0f);
    glEnable(GL_DEPTH_TEST);
    glEnable(GL_TEXTURE_2D);
    glViewport(0, 0, WIDTH, HEIGHT);
}

void DrawSun()
{
    glBindTexture(GL_TEXTURE_2D, texture_sun);
    glBindVertexArray(VAO_sun);

    // вращение вокруг своей оси
    rotate_sun = glm::rotate(rotate_sun, glm::radians(-0.5f), glm::vec3(0.0f, 1.0f, 0.0f));

    model = scale_sun * translate_sun * rotate_sun;
    view = glm::lookAt(camera_pos, camera_pos + camera_front, camera_up);

    glUniformMatrix4fv(Unif_model, 1, GL_FALSE, glm::value_ptr(model));
    glUniformMatrix4fv(Unif_view, 1, GL_FALSE, glm::value_ptr(view));
    glUniformMatrix4fv(Unif_projection, 1, GL_FALSE, glm::value_ptr(projection));

    glDrawArrays(GL_QUADS, 0, vertices_counter_sun);

    glBindVertexArray(0);
    glBindTexture(GL_TEXTURE_2D, 0);
}

void DrawPlanets()
{
    glBindTexture(GL_TEXTURE_2D, texture_planets);
    glBindVertexArray(VAO_planets);

    view = glm::lookAt(camera_pos, camera_pos + camera_front, camera_up);

    glUniformMatrix4fv(Unif_view, 1, GL_FALSE, glm::value_ptr(view));
    glUniformMatrix4fv(Unif_projection, 1, GL_FALSE, glm::value_ptr(projection));

    // вращение вокруг своей оси
    rotate_planets = glm::rotate(rotate_planets, glm::radians(1.0f), glm::vec3(1.0f, 0.0f, 0.5f));

    // вращение вокруг Солнца
    rotate_planets_step = glm::rotate(rotate_planets_step, glm::radians(1.0f), glm::vec3(0.0f, 1.0f, 0.0f));

    for (int i = 0; i < translations_planet.size(); i++)
    {
        translate_planets = glm::translate(glm::mat4(1.0f), translations_planet[i]);
        model = scale_planets * rotate_planets_step * translate_planets * rotate_planets;
        glUniformMatrix4fv(Unif_model, 1, GL_FALSE, glm::value_ptr(model));

        glDrawArrays(GL_QUADS, 0, vertices_counter_planets);
    }

    glBindVertexArray(0);
    glBindTexture(GL_TEXTURE_2D, 0);
}

void Draw() {
    glUseProgram(Program);
    
    DrawSun();
    DrawPlanets();

    glUseProgram(0);

    checkOpenGLerror();
}