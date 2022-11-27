#include <gl/glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
#include <iostream>

#define PI 3.14159265358979323846 

GLuint ProgramConst, ProgramUnif;
GLuint VAO, VBO;
GLuint Unif_color;
bool IsPentagon, IsFan, IsConst = false;

struct Vertex {
    GLfloat x;
    GLfloat y;
};

const char* VertexShaderSource = R"(
    #version 330 core
    layout (location = 0) in vec3 position;

    void main() {
        gl_Position = vec4(position, 1.0);
    }
)";


const char* FragConstShaderSource = R"(
    #version 330 core
    out vec4 color;
    void main() {
        color = vec4(0.6, 1.0, 0.95, 1.0);
    }
)";

const char* FragUnifShaderSource = R"(
    #version 330 core
    uniform vec4 frag_color;

    out vec4 color;
    void main() {
        color = frag_color;
    }
)";

void Init();
void Draw();
void SwitchColor();
void SwitchFigure();

int main() {
    sf::Window window(sf::VideoMode(600, 600), "My OpenGL window", sf::Style::Default, sf::ContextSettings(24));
    window.setVerticalSyncEnabled(true);

    window.setActive(true);

    glewInit();

    Init();
    glEnable(GL_DEPTH_TEST);

    while (window.isOpen()) {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed) {
                window.close();
            }
            else if (event.type == sf::Event::Resized) {
                glViewport(0, 0, event.size.width, event.size.height);
            }
            else if (event.type == sf::Event::KeyPressed) {
                switch (event.key.code) {
                case (sf::Keyboard::Left): SwitchColor(); break;
                case (sf::Keyboard::Right): SwitchFigure(); break;
                default: break;
                }
            }
        }

        glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        Draw();

        window.display();
    }

    glDeleteProgram(ProgramConst);
    glDeleteProgram(ProgramUnif);
    glDeleteVertexArrays(1, &VAO);
    glDeleteBuffers(1, &VBO);

    return 0;
}

void checkOpenGLerror() {
    GLenum errCode;
    // https://www.khronos.org/opengl/wiki/OpenGL_Error
    if ((errCode = glGetError()) != GL_NO_ERROR)
        std::cout << "OpenGl error!: " << errCode << std::endl;
}

void ShaderLog(unsigned int shader) {
    int infologLen = 0;
    int charsWritten = 0;
    char* infoLog;
    glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &infologLen);
    if (infologLen > 1)
    {
        infoLog = new char[infologLen];
        if (infoLog == NULL)
        {
            std::cout << "ERROR: Could not allocate InfoLog buffer" << std::endl;
            exit(1);
        }
        glGetShaderInfoLog(shader, infologLen, &charsWritten, infoLog);
        std::cout << "InfoLog: " << infoLog << "\n\n\n";
        delete[] infoLog;
    }
}


void InitVAO() {

    float angle = -72 * PI / 180.0;

    Vertex v0 = { 0.0, 0.8 };
    Vertex v1 = { v0.x * cos(angle) - v0.y * sin(angle), v0.x * sin(angle) + v0.y * cos(angle) };
    Vertex v2 = { v1.x * cos(angle) - v1.y * sin(angle), v1.x * sin(angle) + v1.y * cos(angle) };
    Vertex v3 = { v2.x * cos(angle) - v2.y * sin(angle), v2.x * sin(angle) + v2.y * cos(angle) };
    Vertex v4 = { v3.x * cos(angle) - v3.y * sin(angle), v3.x * sin(angle) + v3.y * cos(angle) };

    Vertex v5 = { v1.x + 0.2, v2.y + 0.2 };
    Vertex v6 = { v2.x - 0.15, v2.y };
    Vertex v7 = { v3.x + 0.15, v3.y };
    Vertex v8 = { v4.x - 0.2, v3.y + 0.2 };

    Vertex vertexes[10] = { v0, v1, v2, v3, v4,
                            v0, v5, v6, v7, v8};


    glGenVertexArrays(1, &VAO);
    glGenBuffers(1, &VBO);

    glBindVertexArray(VAO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertexes), vertexes, GL_STATIC_DRAW);

    glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 0, (GLvoid*)0);
    glEnableVertexAttribArray(0);

    glBindBuffer(GL_ARRAY_BUFFER, 0);

    glBindVertexArray(0);

    checkOpenGLerror();
}


void InitShader() {
    GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vShader, 1, &VertexShaderSource, NULL);
    glCompileShader(vShader);
    std::cout << "vertex shader \n";
    ShaderLog(vShader);

    GLuint fConstShader = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fConstShader, 1, &FragConstShaderSource, NULL);
    glCompileShader(fConstShader);
    std::cout << "fragment constant shader \n";
    ShaderLog(fConstShader);

    GLuint fUnifShader = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fUnifShader, 1, &FragUnifShaderSource, NULL);
    glCompileShader(fUnifShader);
    std::cout << "fragment uniform shader \n";
    ShaderLog(fUnifShader);

    ProgramConst = glCreateProgram();
    glAttachShader(ProgramConst, vShader);
    glAttachShader(ProgramConst, fConstShader);
    glLinkProgram(ProgramConst);

    int link_ok;
    glGetProgramiv(ProgramConst, GL_LINK_STATUS, &link_ok);
    if (!link_ok) {
        std::cout << "error attach shaders constant \n";
        return;
    }

    ProgramUnif = glCreateProgram();
    glAttachShader(ProgramUnif, vShader);
    glAttachShader(ProgramUnif, fUnifShader);
    glLinkProgram(ProgramUnif);

    glGetProgramiv(ProgramUnif, GL_LINK_STATUS, &link_ok);
    if (!link_ok) {
        std::cout << "error attach shaders uniform \n";
        return;
    }

    Unif_color = glGetUniformLocation(ProgramUnif, "frag_color");
    if (Unif_color == -1)
    {
        std::cout << "could not bind unif frag color" << std::endl;
        return;
    }

    glDeleteShader(vShader);
    glDeleteShader(fConstShader);
    glDeleteShader(fUnifShader);

    checkOpenGLerror();
}

void Init() {
    InitShader();
    InitVAO();
}

void SwitchColor()
{
    if (IsConst)
    {
        IsConst = false;
        std::cout << "switch to uniform color \n";
        return;
    }
    IsConst = true;
    std::cout << "switch to constant color \n";
}

void SwitchFigure()
{
    if (IsPentagon)
    {
        IsPentagon = false;
        std::cout << "switch to quadrilateral \n";
        return;
    }
    if (IsFan)
    {
        IsFan = false;
        IsPentagon = true;
        std::cout << "switch to pentagon \n";
        return;
    }
    IsFan = true;
    std::cout << "switch to fan \n";
}


void Draw() {
    if (IsConst)
    {
        glUseProgram(ProgramConst);
    }
    else
    {
        glUseProgram(ProgramUnif);
        glUniform4f(Unif_color, 0.8, 0.2, 1.0, 1.0);
    }

    glBindVertexArray(VAO);

    if (IsPentagon)
    {
        glDrawArrays(GL_TRIANGLE_FAN, 0, 5);
    }
    else if (IsFan)
    {
        glDrawArrays(GL_TRIANGLE_FAN, 5, 5);
    }
    else
    {
        glDrawArrays(GL_TRIANGLE_FAN, 3, 4);
    }

    glBindVertexArray(0);
    glUseProgram(0);
    
    checkOpenGLerror();
}
